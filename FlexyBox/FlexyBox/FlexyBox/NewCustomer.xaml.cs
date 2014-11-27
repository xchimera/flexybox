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
            Model = new NewCustomerViewModel()
                {
                    Customer = new CustomerFlow()
                };
            Model.IsNew = true;
            this.customerId = customer;
            this.employeeId = employeeId;

            ReloadProducts();
        }
        public NewCustomer(CustomerFlow customer, int employeeId)
        {
            InitializeComponent();
            Model = new NewCustomerViewModel();
            Model.IsNew = false;
            Model.Customer = customer;
            this.employeeId = employeeId;
            ReloadProducts();
            SetCheckedProducts();
        }

        private void ReloadProducts()
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

        private void SetCheckedProducts()
        {
            foreach (var product in Model.Products)
            {
                foreach (var usedProduct in Model.Customer.Products)
                {
                    if (product.Entity.Id == usedProduct.Id)
                    {
                        product.IsChecked = true;

                    }
                }
            }
        }

        private List<StepAnswer> CreateAnswersForQuestions(CustomerFlow customer, List<int> productIds, int employeeId)
        {
            List<int> questions = new List<int>();
            List<int> currentQuestionsIds;
            if (customer.Answers == null)
                currentQuestionsIds = new List<int>();
            else
                currentQuestionsIds = customer.Answers.Select(x => x.QuestionId).ToList();

            var result = new List<StepAnswer>();

            using (var ctx = new FlexyboxContext())
            {
                questions = ctx.Query<StepQuestion>()
                    .Where(x => productIds.Contains(x.Product.Id) && !currentQuestionsIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();
            }

            foreach (var question in questions)
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
        private bool CheckValidity(List<Product> products)
        {
            bool isValid = true;
            if (products.Count == 0)
            {
                var msg = MessageBox.Show("Er du HELT sikker på at du vil oprette kunden uden produkter?", "Er du sikker?", MessageBoxButton.YesNo);
                if (msg == MessageBoxResult.No)
                    isValid = false;
            }

            if (Model.CustomerName == string.Empty)
            {
                var msg = MessageBox.Show("Er du HELT sikker på at du vil oprette kunden uden navn?", "Er du sikker?", MessageBoxButton.YesNo);
                if (msg == MessageBoxResult.No)
                    isValid = false;
            }
            return isValid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var checkedProducts = new List<Product>();
            foreach (var product in Model.Products)
            {
                if (product.IsChecked)
                {
                    checkedProducts.Add(product.Entity);
                }
            }
            if (!CheckValidity(checkedProducts))
                return;

            if (Model.IsNew)
            {
                Model.Customer.CustomerId = customerId;
                Model.Customer.Name = Model.CustomerName;
                Model.Customer.Products = checkedProducts;
            }
            else
                Model.Customer.Products = checkedProducts;


            if (Model.IsNew)
                Model.Customer.Answers = CreateAnswersForQuestions(Model.Customer, checkedProducts.Select(x => x.Id).ToList(), employeeId);

            else
            {
                var answers = CreateAnswersForQuestions(Model.Customer, checkedProducts.Select(x => x.Id).ToList(), employeeId);
                foreach (var answer in answers)
                {
                    Model.Customer.Answers.Add(answer);
                }
            }
            var answersToAdd = Model.Customer.Answers.Where(x=> x.Id == 0).ToList();

            using (var ctx = new FlexyboxContext())
            {
                foreach (var product in Model.Customer.Products)
                {
                    ctx.Entry(product).State = EntityState.Unchanged;
                }

                answersToAdd.ForEach(x => ctx.Entry(x).State = EntityState.Added);

                foreach (var answer in Model.Customer.Answers)
                {
                    if (answer.Id != 0)
                        ctx.Entry(answer).State = EntityState.Unchanged;
                }
                
                if (!ctx.SaveEntity<CustomerFlow>(Model.Customer))
                {
                    MessageBox.Show("Fejl i at oprette kunde!");
                    return;
                }
            }
            DialogResult = true;
            Close();

        }
    }
    public class NewCustomerViewModel
    {
        public BindingList<ProductClickViewModel> Products { get; set; }
        public CustomerFlow Customer { get; set; }
        public string CustomerName
        {
            get
            {
                if (Customer != null)
                    return Customer.Name;
                return "";
            }
            set
            {
                Customer.Name = value;
            }
        }
        public bool IsNew { get; set; }
        public string Header
        {
            get
            {
                if (IsNew)
                    return "Du har valgt en ny kunde";
                return "Rediger kunde";
            }
        }

        public string SaveText
        {
            get
            {
                if (IsNew)
                    return "Opret";
                return "Gem";
            }
        }

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
