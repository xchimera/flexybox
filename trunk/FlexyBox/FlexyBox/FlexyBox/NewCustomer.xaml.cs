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
    /// 
    //lavet af Søren
    public partial class NewCustomer : Window
    {
        int customerId;
        int employeeId;
        public NewCustomerViewModel Model
        {
            get { return DataContext as NewCustomerViewModel; }
            set { DataContext = value; }
        }

        //ny kunde
        public NewCustomer(int customer, int employeeId)
        {
            InitializeComponent();
            //sæt datacontext og lav nyt kunde objekt
            Model = new NewCustomerViewModel()
                {
                    Customer = new CustomerFlow()
                };
            //sæt vinduet til at det er en ny kunde
            Model.IsNew = true;
            this.customerId = customer;
            this.employeeId = employeeId;

            ReloadProducts();
        }
        //rediger kunde
        public NewCustomer(CustomerFlow customer, int employeeId)
        {
            InitializeComponent();
            Model = new NewCustomerViewModel();
            //sæt vinduet til at kunden skal redigeres
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
                //hent alle produkter
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
            //kør igennem alle produkter og marker dem der er valgt ved rediger kunde
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
            //hvis ikke kunden har svar på sig, skab en ny liste ellers hent spørgsmålenes Id ud
            if (customer.Answers == null)
                currentQuestionsIds = new List<int>();
            else
                currentQuestionsIds = customer.Answers.Select(x => x.QuestionId).ToList();

            var result = new List<StepAnswer>();

            using (var ctx = new FlexyboxContext())
            {
                //hent alle de spørgsmål ud som kunden endnu ikke er tilknyttet og som hører til de produkter kunden har valgt
                questions = ctx.Query<StepQuestion>()
                    .Where(x => productIds.Contains(x.Product.Id) && !currentQuestionsIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();
            }

            foreach (var question in questions)
            {
                //skab nye svar til de nye spørgsmål
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
                //hvis kunden ikke har valgt nogen produkter, giv en advarsel
                var msg = MessageBox.Show("Er du HELT sikker på at du vil oprette kunden uden produkter?", "Er du sikker?", MessageBoxButton.YesNo);
                if (msg == MessageBoxResult.No)
                    isValid = false;
            }

            if (Model.CustomerName == string.Empty)
            {
                //hvis kunden ikke har et navn, giv en advarsel
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
                //lav en liste af de produkter der er valgt og tilføj dem
                if (product.IsChecked)
                {
                    checkedProducts.Add(product.Entity);
                }
            }
            //hvis kunden ikke var korrekt udfyldt og brugeren ikke ville gå videre, bryd ud af metoden.
            if (!CheckValidity(checkedProducts))
                return;

            //hvis kunden er ny, sæt de relevante properties
            if (Model.IsNew)
            {
                Model.Customer.CustomerId = customerId;
                Model.Customer.Name = Model.CustomerName;
                Model.Customer.Products = checkedProducts;
            }
                //hvis kunden ikke er ny skal de produkterne bare sættes
            else
                Model.Customer.Products = checkedProducts;

            //hvis kunden er ny skal alle svar bare sættes
            if (Model.IsNew)
                Model.Customer.Answers = CreateAnswersForQuestions(Model.Customer, checkedProducts.Select(x => x.Id).ToList(), employeeId);

            else
            {
                //hvis kunden ikke er ny skal man iterere igennem svarene og tilføj disse
                var answers = CreateAnswersForQuestions(Model.Customer, checkedProducts.Select(x => x.Id).ToList(), employeeId);
                foreach (var answer in answers)
                {
                    Model.Customer.Answers.Add(answer);
                }
            }
            //find de nye svar og tilføj dem til en liste til at blive lagt i databasen
            var answersToAdd = Model.Customer.Answers.Where(x=> x.Id == 0).ToList();

            using (var ctx = new FlexyboxContext())
            {
                foreach (var product in Model.Customer.Products)
                {
                    //sæt at produkterne ikke skal gemmes i databasen igen
                    ctx.Entry(product).State = EntityState.Unchanged;
                }
                //sæt de nye spørgsmål til at de er blevet tilføjet og skal gemmes i databasen
                answersToAdd.ForEach(x => ctx.Entry(x).State = EntityState.Added);

                foreach (var answer in Model.Customer.Answers)
                {
                    if (answer.Id != 0)
                        //alle svar der findes i forvejen skal ikke gemmes i databasen
                        ctx.Entry(answer).State = EntityState.Unchanged;
                }
                //gem kunden og lav tjek på om det lykkedes
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
