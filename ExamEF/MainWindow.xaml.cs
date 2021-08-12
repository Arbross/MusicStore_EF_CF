using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ExamEF
{
    public partial class MainWindow : Window
    {
        public LoginRegisterService service { get; private set; } = new LoginRegisterService();
        private Plate tmp = null;
        public MainWindow()
        {
            InitializeComponent();

            service.ctx.Users.Add(new User() { Login = "Test", Password = LoginRegisterService.ComputeSha256Hash("1"), IsAdmin = true });
            service.ctx.SaveChanges();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (service.ctx.Users == null)
            {
                MessageBox.Show("Database is successfuly created. Try again!");
                return;
            }

            if (WinRegister.Visibility == Visibility.Hidden)
            {
                WinLogin.Visibility = Visibility.Hidden;
                WinRegister.Visibility = Visibility.Visible;
            }
            else
            {
                service.Register(LoginTB.Text, PasswordTB.Text);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (service.ctx.Users == null)
            {
                MessageBox.Show("Database is successfuly created. Try again!");
                return;
            }

            if (WinLogin.Visibility == Visibility.Hidden)
            {
                WinRegister.Visibility = Visibility.Hidden;
                WinLogin.Visibility = Visibility.Visible;
            }
            else
            {
                service.Login(LoginTBR.Text, LoginRegisterService.ComputeSha256Hash(PasswordTBR.Text));
                if (service.IsLogin == true)
                {
                    WinMain.Visibility = Visibility.Hidden;
                    WinPlate.Visibility = Visibility.Visible;
                    if (service.user.IsAdmin == true)
                    {
                        AT.Visibility = Visibility.Visible;
                    }
                    IsAvailableSort();
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bye!");
            Application.Current.Shutdown();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            Plate obj = ShowPanel.SelectedItem as Plate;

            service.ctx.Plates.Remove(obj);
            service.ctx.SoldPlates.Add(new SoldPlate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = obj.GroupId, MakerId = obj.MakerId, UserId = service.user.Id, GenreId = obj.GenreId, SoldTime = DateTime.UtcNow });

            if (service.user.PermanentDiscount == null)
            {
                double allPrice = 0;
                foreach (SoldPlate item in service.ctx.SoldPlates)
                {
                    if (item.UserId == service.user.Id)
                    {
                        allPrice += item.Price;
                        if (allPrice >= 10000)
                        {
                            User user = service.ctx.Users.Where(x => x.Id == service.user.Id).SingleOrDefault();
                            user.PermanentDiscount = 10;
                        }
                    }
                }
            }

            service.ctx.SaveChanges();
            IsAvailableSort();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            RightMain.Visibility = Visibility.Hidden;
            RightSort.Visibility = Visibility.Visible;
        }

        private void Back_Sort_Click(object sender, RoutedEventArgs e)
        {
            RightSort.Visibility = Visibility.Hidden;
            RightMain.Visibility = Visibility.Visible;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            WinPlate.Visibility = Visibility.Hidden;
            WinMain.Visibility = Visibility.Visible;
            AT.Visibility = Visibility.Hidden;
            service.LogOut();
        }

        private void AT_Click(object sender, RoutedEventArgs e)
        {
            RightMain.Visibility = Visibility.Hidden;
            RightAT.Visibility = Visibility.Visible;
            Standart_Click(null, null);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            RightAT.Visibility = Visibility.Hidden;
            RightMain.Visibility = Visibility.Visible;
            IsAvailableSort();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            List<Plate> plate = new List<Plate>();
            foreach (Plate item in service.ctx.Plates.OrderByDescending(x => x.PublishYear).ToList())
            {
                if (item.IsAvailable == false)
                {
                    plate.Add(item);
                }
            }

            ShowPanel.ItemsSource = plate;
        }

        private void MoreSale_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            List<Plate> plate = new List<Plate>();
            foreach (Plate item in service.ctx.Plates.OrderByDescending(x => x.SoldCopies).ToList())
            {
                if (item.IsAvailable == false)
                {
                    plate.Add(item);
                }
            }

            ShowPanel.ItemsSource = plate;
        }

        private void PopularAuthors_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            List<Plate> plate = new List<Plate>();
            foreach (Plate item in service.ctx.Plates.OrderByDescending(x => x.CountOfTrecks).ToList())
            {
                if (item.IsAvailable == false)
                {
                    plate.Add(item);
                }
            }

            ShowPanel.ItemsSource = plate;
        }

        private void PopularGenresPerDay_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            ShowPanelOtherGenres.Visibility = Visibility.Visible;

            List<Genre> genres = new List<Genre>();
            foreach (SoldPlate item in service.ctx.SoldPlates.Where(x => x.SoldTime.Day == DateTime.UtcNow.Day && x.SoldTime.Month == DateTime.UtcNow.Month && x.SoldTime.Year == DateTime.UtcNow.Year).ToList())
            {
                if (item.IsAvailable == false && item.GenreId != null)
                {
                    genres.Add(service.ctx.Genres.Where(x => x.Id == item.GenreId).SingleOrDefault());
                }
            }

            ShowPanelOtherGenres.ItemsSource = genres;
        }

        private void PopularGenresPerWeek_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            ShowPanelOtherGenres.Visibility = Visibility.Visible;

            List<Genre> genres = new List<Genre>();
            foreach (SoldPlate item in service.ctx.SoldPlates.Where(x => x.SoldTime.Day >= DateTime.UtcNow.Day && x.SoldTime.Day < DateTime.UtcNow.Day && x.SoldTime.Year == DateTime.UtcNow.Year).ToList())
            {
                if (item.IsAvailable == false && item.GenreId != null)
                {
                    genres.Add(service.ctx.Genres.Where(x => x.Id == item.GenreId).SingleOrDefault());
                }
            }

            ShowPanelOtherGenres.ItemsSource = genres;
        }

        private void PopularGenresPerMonth_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            ShowPanelOtherGenres.Visibility = Visibility.Visible;

            List<Genre> genres = new List<Genre>();
            foreach (SoldPlate item in service.ctx.SoldPlates.Where(x => x.SoldTime.Month == DateTime.UtcNow.Month && x.SoldTime.Year == DateTime.UtcNow.Year).ToList())
            {
                if (item.IsAvailable == false && item.GenreId != null)
                {
                    genres.Add(service.ctx.Genres.Where(x => x.Id == item.GenreId).SingleOrDefault());
                }
            }

            ShowPanelOtherGenres.ItemsSource = genres;
        }

        private void PopularGenresPerYear_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            ShowPanelOtherGenres.Visibility = Visibility.Visible;

            List<Genre> genres = new List<Genre>();
            foreach (SoldPlate item in service.ctx.SoldPlates.Where(x => x.SoldTime.Year == DateTime.UtcNow.Year).ToList())
            {
                if (item.IsAvailable == false && item.GenreId != null)
                {
                    genres.Add(service.ctx.Genres.Where(x => x.Id == item.GenreId).SingleOrDefault());
                }
            }

            ShowPanelOtherGenres.ItemsSource = genres;
        }

        private void IsAvailableSort()
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            if (service.ctx.Plates == null)
            {
                return;
            }

            List<Plate> plate = new List<Plate>();
            foreach (Plate item in service.ctx.Plates.ToList())
            {
                if (item.IsAvailable == false)
                {
                    plate.Add(item);
                }
            }

            ShowPanel.ItemsSource = plate;
        }

        private void Standart_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel.ItemsSource = service.ctx.Plates.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RightAdd.Visibility = Visibility.Visible;
            RightAT.Visibility = Visibility.Hidden;

            GroupId.ItemsSource = service.ctx.Groups.Select(x => x.Name).ToList();
            MakerId.ItemsSource = service.ctx.Makers.Select(x => x.Name).ToList();
            GenreId.ItemsSource = service.ctx.Genres.Select(x => x.Name).ToList();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            service.ctx.Plates.Remove(ShowPanel.SelectedItem as Plate);
            service.ctx.SaveChanges();
            Standart_Click(null, null);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem != null)
            {
                RightEdit.Visibility = Visibility.Visible;
                RightAT.Visibility = Visibility.Hidden;

                tmp = ShowPanel.SelectedItem as Plate;
                NameEdit.Text = tmp.Name;
                CountOfTrecksEdit.Text = Convert.ToString(tmp.CountOfTrecks);
                PublishYearEdit.Text = Convert.ToString(tmp.PublishYear);
                LocalPriceEdit.Text = Convert.ToString(tmp.LocalPrice);
                PriceEdit.Text = Convert.ToString(tmp.Price);
                SoldCopiesEdit.Text = Convert.ToString(tmp.SoldCopies);
                GroupIdEdit.Text = Convert.ToString(tmp.GroupId);
                MakerIdEdit.Text = Convert.ToString(tmp.MakerId);

                GroupIdEdit.ItemsSource = service.ctx.Groups.Select(x => x.Name).ToList();
                MakerIdEdit.ItemsSource = service.ctx.Makers.Select(x => x.Name).ToList();
            }
            else
            {
                MessageBox.Show("Selected item to edit is equeal null.");
            }
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            Plate obj = ShowPanel.SelectedItem as Plate;

            service.ctx.Plates.Remove(obj);
            service.ctx.SoldPlates.Add(new SoldPlate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = obj.GroupId, MakerId = obj.MakerId, GenreId = obj.GenreId, SoldTime = DateTime.UtcNow, UserId = null });
            service.ctx.SaveChanges();
            Standart_Click(null, null);
        }

        private void Drop_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            Plate obj = ShowPanel.SelectedItem as Plate;

            if (obj != null)
            {
                if (obj.IsAvailable == true)
                {
                    obj.IsAvailable = false;
                }
                else
                {
                    obj.IsAvailable = true;
                }
            }
            else
            {
                MessageBox.Show("The list of items is emty.");
                return;
            }

            service.ctx.SaveChanges();
            Standart_Click(null, null);
        }

        private void Discount_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            RightDiscount.Visibility = Visibility.Visible;
            RightAT.Visibility = Visibility.Hidden;

            tmp = ShowPanel.SelectedItem as Plate;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (Plate item in service.ctx.Plates)
            {
                if (item.Name == Name.Text)
                {
                    MessageBox.Show("Name of product isn't available.");
                    return;
                }
            }

            service.ctx.Plates.Add(new Plate() { Name = Name.Text, CountOfTrecks = Convert.ToInt32(CountOfTrecks.Text), PublishYear = Convert.ToInt32(PublishYear.Text), LocalPrice = Convert.ToDouble(LocalPrice.Text), Price = Convert.ToDouble(Price.Text), SoldCopies = Convert.ToInt32(SoldCopies.Text), GroupId = GroupId.SelectedIndex + 1, MakerId = MakerId.SelectedIndex + 1, GenreId = GenreId.SelectedIndex + 1 });
            service.ctx.SaveChanges();
            Standart_Click(null, null);
        }

        private void btnBackAdd_Click(object sender, RoutedEventArgs e)
        {
            RightAT.Visibility = Visibility.Visible;
            RightAdd.Visibility = Visibility.Hidden;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Plate result = service.ctx.Plates.Single(x => x.Id == tmp.Id);
            if (result != null)
            {
                if (NameEdit.Text == String.Empty || CountOfTrecksEdit.Text == String.Empty || PublishYearEdit.Text == String.Empty || LocalPriceEdit.Text == String.Empty || PriceEdit.Text == String.Empty || SoldCopiesEdit.Text == String.Empty)
                {
                    MessageBox.Show("Some fields may be empty.");
                    return;
                }

                if (service.ctx.Groups.Select(x => x.Id == GroupIdEdit.SelectedIndex + 1).Single() == false || service.ctx.Makers.Select(x => x.Id == MakerIdEdit.SelectedIndex + 1).Single() == false)
                {
                    MessageBox.Show("It is no exists combo box id item like this.");
                    return;
                }

                result.Name = NameEdit.Text;

                int.TryParse(CountOfTrecksEdit.Text, out int cot);
                result.CountOfTrecks = cot;

                int.TryParse(PublishYearEdit.Text, out int py);
                result.PublishYear = py;

                double.TryParse(LocalPriceEdit.Text, out double lp);
                result.LocalPrice = lp;

                double.TryParse(PriceEdit.Text, out double p);
                result.Price = p;

                int.TryParse(SoldCopiesEdit.Text, out int sc);
                result.SoldCopies = sc;

                result.GroupId = GroupIdEdit.SelectedIndex + 1;
                result.MakerId = MakerIdEdit.SelectedIndex + 1;

                service.ctx.SaveChanges();
                Standart_Click(null, null);
            }
            else
            {
                MessageBox.Show("The list of items is emty.");
                return;
            }
        }

        private void btnBackEdit_Click(object sender, RoutedEventArgs e)
        {
            RightAT.Visibility = Visibility.Visible;
            RightEdit.Visibility = Visibility.Hidden;
            tmp = null;
        }

        private void btnDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountPercent.Text == String.Empty)
            {
                MessageBox.Show("Some fields may be empty.");
                return;
            }

            if (Convert.ToDouble(DiscountPercent.Text).GetType() == typeof(double))
            {
                Plate result = service.ctx.Plates.Single(x => x.Id == tmp.Id);

                if (result.BeforeDiscountPrice == null)
                {
                    result.BeforeDiscountPrice = result.Price;
                }

                double.TryParse(DiscountPercent.Text, out double dp);
                dp += (double)service.user.PermanentDiscount;

                if (dp > 100)
                {
                    dp = 100;
                }

                if (dp == 100)
                {
                    result.Price = 0;
                }
                else
                {
                    result.Price = result.Price * (dp / 100);
                }

                service.ctx.SaveChanges();
                Standart_Click(null, null);
            }
            else
            {
                MessageBox.Show("The value isn't number percent type.");
                return;
            }
        }

        private void btnDiscount_Back_Click(object sender, RoutedEventArgs e)
        {
            RightAT.Visibility = Visibility.Visible;
            RightDiscount.Visibility = Visibility.Hidden;
            tmp = null;
        }

        private void btnDiscountRemove_Click(object sender, RoutedEventArgs e)
        {
            Plate result = service.ctx.Plates.Single(x => x.Id == tmp.Id);

            if (result.BeforeDiscountPrice != null)
            {
                result.Price = (double)result.BeforeDiscountPrice;
                result.BeforeDiscountPrice = null;
                Standart_Click(null, null);
            }
            else
            {
                MessageBox.Show("Discount is already equeal zero.");
                return;
            }
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            Plate obj = ShowPanel.SelectedItem as Plate;

            service.ctx.Basket.Add(new Basket() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = (int)obj.GroupId, MakerId = (int)obj.MakerId, UserId = service.user.Id, GenreId = obj.GenreId });
            service.ctx.Plates.Remove(obj);
            service.ctx.SaveChanges();
            IsAvailableSort();
        }

        private void BasketSort_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            ShowPanelOther.Visibility = Visibility.Visible;
            ShowPanel.SelectedItem = null;
            ShowPanelOther.ItemsSource = service.ctx.Basket.ToList();
        }

        private void NStandart_Click(object sender, RoutedEventArgs e)
        {
            IsAvailableSort();
        }

        private void BuyFromBasket_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                if (ShowPanelOther.SelectedItem == null)
                {
                    MessageBox.Show("Selected item is null to edit it.");
                    return;
                }

                Basket obj = ShowPanelOther.SelectedItem as Basket;

                service.ctx.SoldPlates.Add(new SoldPlate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = obj.GroupId, MakerId = obj.MakerId, UserId = service.user.Id, GenreId = obj.GenreId, SoldTime = DateTime.UtcNow });
                service.ctx.Basket.Remove(obj);

                if (service.user.PermanentDiscount == null)
                {
                    double allPrice = 0;
                    foreach (SoldPlate item in service.ctx.SoldPlates)
                    {
                        if (item.UserId == service.user.Id)
                        {
                            allPrice += item.Price;
                            if (allPrice >= 10000)
                            {
                                User user = service.ctx.Users.Where(x => x.Id == service.user.Id).SingleOrDefault();
                                user.PermanentDiscount = 10;
                            }
                        }
                    }
                }

                service.ctx.SaveChanges();
                ShowPanelOther.ItemsSource = service.ctx.Basket.ToList();
            }
            else
            {
                MessageBox.Show("Please select basket sort.");
            }
        }

        private void DiscountGenre_Click(object sender, RoutedEventArgs e)
        {
            RightAT.Visibility = Visibility.Hidden;
            RightDiscountGenre.Visibility = Visibility.Visible;
        }

        private void btnDiscount_Back_Two_Click(object sender, RoutedEventArgs e)
        {
            RightDiscountGenre.Visibility = Visibility.Hidden;
            RightAT.Visibility = Visibility.Visible;
        }

        private void btnDiscountTwo_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountPercentTwo.Text == String.Empty || DiscountGenreOne.Text == String.Empty)
            {
                MessageBox.Show("Some fields may be empty.");
                return;
            }

            if (Convert.ToDouble(DiscountPercentTwo.Text).GetType() == typeof(double))
            {
                double.TryParse(DiscountPercentTwo.Text, out double dp);
                dp += (double)service.user.PermanentDiscount;

                List<Plate> plate = new List<Plate>();

                foreach (Plate item in service.ctx.Plates)
                {
                    if (item.Genre.Name == DiscountGenreOne.Text)
                    {
                        plate.Add(item);
                    }
                }

                foreach (Plate item in plate)
                {
                    if (item.BeforeDiscountPrice == null)
                    {
                        item.BeforeDiscountPrice = item.Price;
                    }

                    if (dp > 100)
                    {
                        dp = 100;
                    }

                    if (dp == 100)
                    {
                        item.Price = 0;
                    }
                    else
                    {
                        item.Price = item.Price * (dp / 100);
                    }
                }

                service.ctx.SaveChanges();
                Standart_Click(null, null);
            }
            else
            {
                MessageBox.Show("The value isn't number percent or string type.");
                return;
            }
        }

        private void btnDiscountRemoveTwo_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountGenreOne.Text == String.Empty)
            {
                MessageBox.Show("Genre field is empty.");
                return;
            }

            foreach (Plate item in service.ctx.Plates)
            {
                if (item.Genre.Name == DiscountGenreOne.Text)
                {
                    if (item.BeforeDiscountPrice != null)
                    {
                        item.Price = (double)item.BeforeDiscountPrice;
                        item.BeforeDiscountPrice = null;
                    }
                }
            }

            service.ctx.SaveChanges();
            Standart_Click(null, null);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            RightMain.Visibility = Visibility.Hidden;
            RightSearch.Visibility = Visibility.Visible;
        }

        private void btnNameDiskFind_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            ShowPanelOtherSearch.Visibility = Visibility.Visible;

            List<Plate> plates = new List<Plate>();
            foreach (Plate item in service.ctx.Plates)
            {
                if (item.Name.Contains(SearchField.Text))
                {
                    plates.Add(item);
                }
            }

            ShowPanelOtherSearch.ItemsSource = plates;
        }

        private void btnMakerDiskFind_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            ShowPanelOtherSearch.Visibility = Visibility.Visible;

            List<Maker> makers = new List<Maker>();
            foreach (Maker item in service.ctx.Makers)
            {
                if (item.Name.Contains(SearchField.Text))
                {
                    makers.Add(item);
                }
            }

            ShowPanelOtherSearch.ItemsSource = makers;
        }

        private void btnGenreDiskFind_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSavePlate.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSavePlate.Visibility = Visibility.Hidden;
            }

            ShowPanelOtherSearch.Visibility = Visibility.Visible;

            List<Genre> genres = new List<Genre>();
            foreach (Genre item in service.ctx.Genres)
            {
                if (item.Name.Contains(SearchField.Text))
                {
                    genres.Add(item);
                }
            }

            ShowPanelOtherSearch.ItemsSource = genres;
        }

        private void btnBackDiskFind_Click(object sender, RoutedEventArgs e)
        {
            RightSearch.Visibility = Visibility.Hidden;
            RightMain.Visibility = Visibility.Visible;
        }

        private void btnSavePlate_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanel.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            Plate obj = ShowPanel.SelectedItem as Plate;

            int GenreId = (int)obj.GenreId;
            int MakerId = (int)obj.MakerId;
            int GroupId = (int)obj.GroupId;

            service.ctx.Plates.Remove(obj);
            service.ctx.SavePlates.Add(new SavePlate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = GroupId, MakerId = MakerId, UserId = service.user.Id, GenreId = GenreId });
            

            service.ctx.SaveChanges();
            IsAvailableSort();
        }

        private void btnShowSavedPlates_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOther.Visibility == Visibility.Visible)
            {
                ShowPanelOther.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherSearch.Visibility == Visibility.Visible)
            {
                ShowPanelOtherSearch.Visibility = Visibility.Hidden;
            }

            if (ShowPanelOtherGenres.Visibility == Visibility.Visible)
            {
                ShowPanelOtherGenres.Visibility = Visibility.Hidden;
            }

            ShowPanelOtherSavePlate.Visibility = Visibility.Visible;

            List<SavePlate> savePlates = new List<SavePlate>();
            foreach (SavePlate item in service.ctx.SavePlates)
            {
                if (item.UserId == service.user.Id)
                {
                    savePlates.Add(item);
                }
            }

            ShowPanelOtherSavePlate.ItemsSource = savePlates;
        }

        private void btnCancelSavePlates_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOtherSavePlate.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            SavePlate obj = ShowPanelOtherSavePlate.SelectedItem as SavePlate;

            service.ctx.SavePlates.Remove(obj);
            service.ctx.Plates.Add(new Plate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = obj.GroupId, MakerId = obj.MakerId, GenreId = obj.GenreId });

            service.ctx.SaveChanges();
            btnShowSavedPlates_Click(null, null);
        }

        private void btnBuySavePlates_Click(object sender, RoutedEventArgs e)
        {
            if (ShowPanelOtherSavePlate.SelectedItem == null)
            {
                MessageBox.Show("Selected item is null to edit it.");
                return;
            }

            SavePlate obj = ShowPanelOtherSavePlate.SelectedItem as SavePlate;

            service.ctx.SavePlates.Remove(obj);
            service.ctx.SoldPlates.Add(new SoldPlate() { Name = obj.Name, CountOfTrecks = obj.CountOfTrecks, BeforeDiscountPrice = obj.BeforeDiscountPrice, LocalPrice = obj.LocalPrice, Price = obj.Price, PublishYear = obj.PublishYear, SoldCopies = obj.SoldCopies, GroupId = obj.GroupId, MakerId = obj.MakerId, UserId = service.user.Id, GenreId = obj.GenreId, SoldTime = DateTime.UtcNow });

            if (service.user.PermanentDiscount == null)
            {
                double allPrice = 0;
                foreach (SoldPlate item in service.ctx.SoldPlates)
                {
                    if (item.UserId == service.user.Id)
                    {
                        allPrice += item.Price;
                        if (allPrice >= 10000)
                        {
                            User user = service.ctx.Users.Where(x => x.Id == service.user.Id).SingleOrDefault();
                            user.PermanentDiscount = 10;
                        }
                    }
                }
            }

            service.ctx.SaveChanges();
            btnShowSavedPlates_Click(null, null);
        }

        private void btnBackSavePlates_Click(object sender, RoutedEventArgs e)
        {
            RightSavePlate.Visibility = Visibility.Hidden;
            RightMain.Visibility = Visibility.Visible;
        }

        private void SavePlate_Click(object sender, RoutedEventArgs e)
        {
            RightMain.Visibility = Visibility.Hidden;
            RightSavePlate.Visibility = Visibility.Visible;
        }
    }
}
