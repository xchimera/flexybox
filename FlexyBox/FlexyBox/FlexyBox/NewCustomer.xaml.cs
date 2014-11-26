using FlexyBox.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlexyDomain;
using FlexyDomain.Models;
using FlexyDomain.Extensions;
using System.Data.Entity;

namespace FlexyBox
{
    /// <summary>
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class NewCustomer : Window
    {
        int customerId;
        int employeeId;
        public NewCustomerViewModel Model
        {
            get { return DataContext as NewCustomerViewModel; }
            set { DataContext = value; }
        }

        public NewCustomer(int customer, int employeeId)
        {
            InitializeComponent();
            Model = new NewCustomerViewModel();
            this.customerId = customer;
            this.employeeId = employeeId;
            Reload();
        }

        private void Reload()
        {
            var result = new List<ProductClickViewModel>();
            using (var ctx = new FlexyboxContext())
            {
                result = ctx.Query<Product>()
                    .Select(x => new ProductClickViewModel()
                    {
                        Entity = x,
                    }).ToList();
            }
            Model.Products = result.ToBindingList();
        }

        private List<StepAnswer> CreateAnswersForQuestions(CustomerFlow customer, List<int> productIds, int employeeId)
        {
            List<int> questions = new List<int>();
            var result = new List<StepAnswer>();

            using(var ctx = new FlexyboxContext())
            {
                questions = ctx.Query<StepQuestion>()
                    .Where(x => productIds.Contains(x.Product.Id))
                    .Select(x=> x.Id)
                    .ToList();
            }

            foreach(var question in questions)
            {
                result.Add(new StepAnswer()
                    {
                        CustomerFlow = customer,
                        QuestionId = question,
                        TimeChanged = DateTime.Now,
                        EmployeeId = employeeId,
                    });
            }

                return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var checkedProducts = new List<Product>();
            foreach(var product in Model.Products)
            {
                if(product.IsChecked)
                {
                    checkedProducts.Add(product.Entity);
                }
            }

            var customer = new CustomerFlow()
            {
                CustomerId = customerId,
                Name = Model.CustomerName,
                Products = checkedProducts,
            };
            customer.Answers = CreateAnswersForQuestions(customer, checkedProducts.Select(x=> x.Id).ToList(), employeeId);

            using (var ctx = new FlexyboxContext())
            {
                foreach(var product in customer.Products)
                {
                    ctx.Entry(product).State = EntityState.Unchanged;
                }
                if (!ctx.SaveEntity<CustomerFlow>(customer))
                {
                    MessageBox.Show("Fejl i at oprette kunde!");
                    return;
                }
            }
            Close();
            
        }
    }
    public class NewCustomerViewModel
    {
        public BindingList<ProductClickViewModel> Products { get; set; }
        public string CustomerName { get; set; }
        
        public NewCustomerViewModel()
        {
            Products = new BindingList<ProductClickViewModel>();
        }
    }

    public class ProductClickViewModel
    {
        public Product Entity { get; set; }
        public string Name
        {
            get { return Entity.Name; }
        }
        public bool IsChecked { get; set; }
    }
}
