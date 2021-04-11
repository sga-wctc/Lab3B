using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Lab3B
{
    public partial class MainPage : ContentPage
    {
        int legalAge = 21;
        public MainPage()
        {
            InitializeComponent();
            birthdayPicker.Date = DateTime.Today.AddYears(-18);
            RadioButton1.IsChecked = true;
         }

        void Btn_Clicked(object sender, System.EventArgs e)
        {
            Calculate_Wait();
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            ResetLblyears();
        }

        private void ResetLblyears()
        {
            Lbl_years.Text = "Calculate Years until permitted to dring alcohol - Press Calculate";
        }

        void OnCountryCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            ageLabel.Text = $"You have chosen: {button.Content} {button.Value} yrs old legal age ";
            legalAge = Int32.Parse(button.Value.ToString());
            ResetLblyears();
        }

        private void Calculate_Wait()
        {
            int theAge = CalculateAge(birthdayPicker.Date);

            if (theAge >= legalAge)
            {
                Lbl_years.Text = $"You are of legal age to buy alcohol";
            }
            else
            {
                DateTime dateOfLegalAge = DateTime.Today.AddYears(-legalAge);
                string timeUntilAlcohol = AgeDiffCalculation(dateOfLegalAge, birthdayPicker.Date );
                //string timeUntilAlcohol = "?";
                Lbl_years.Text = $"You have {timeUntilAlcohol} to wait";
            }
        }

        public string AgeDiffCalculation(DateTime legalDate, DateTime birthDate)
        {
            if (birthDate < legalDate)
                return "should already be legal age";

            var diff = birthDate - legalDate;
            //  get difference in days
            var diffDays = (int)diff.TotalDays;

            if (diffDays > 7)
            {
                // greater than a week 
                var yearDiff = birthDate.Year - legalDate.Year;

                if (legalDate > birthDate.AddYears(-yearDiff))
                    yearDiff--;

                var d = legalDate;
                var monthDiff = 1;

                while (d.AddMonths(monthDiff) < birthDate.AddYears(-yearDiff))
                    monthDiff++;

                monthDiff--;

                var daysDiff = birthDate.AddYears(-yearDiff).AddMonths(-monthDiff) - legalDate;

                var days = (int)daysDiff.TotalDays;

                return yearDiff + (yearDiff > 1 ? " years " : " year ") + 
                                monthDiff + (monthDiff > 1 ? " months " : " month ") + days + (days > 1 ? " days" : " day");
            }
            else if (diffDays > 0)
            {  // time is less than a week.
                var age = diffDays;
                return age + (age > 1 ? " days" : " day");
            }
            else
            {  // 0 days old? 
                return "too young";
            }
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            // difference in years on dates
            int years = today.Year - birthDate.Year;

            // check month 
            if (birthDate.Month == today.Month && today.Day < birthDate.Day  // if current month and day less than today.
                || today.Month < birthDate.Month) // OR if month after today
            {
                // Reduce age by 1 - birthday had not happened yet this year.
                years--;
            }

            return years;
        }
    }
}
