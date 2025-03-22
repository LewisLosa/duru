using Common.Library;
using Duru.Library.ViewModels;
using Duru.UI.Data.Entities;
using Duru.UI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Duru.UI.ViewModel
{
  public sealed class LoginPageViewModel : ViewModelBase
  {
    public LoginPageViewModel()
    {
      DisplayStatusMessage("Login to Application");

      Entity = new Employee() {
        Email = Environment.UserName
      };
    }

    private Employee _employeeEntity;

    public Employee Entity
    {
      get { return _employeeEntity; }
      set {
        _employeeEntity = value;
        RaisePropertyChanged("Entity");
      }
    }
    public bool Validate()
    {
      bool ret = false;

      Entity.IsLoggedIn = false;
      ValidationMessages.Clear();
      if (string.IsNullOrEmpty(Entity.Email)) {
        AddValidationMessage("Email", "E-Mail Must Be Filled In");
      }
      if (string.IsNullOrEmpty(Entity.Password)) {
        AddValidationMessage("Password", "Password Must Be Filled In");
      }

      ret = (ValidationMessages.Count == 0);

      return ret;
    }

    public bool ValidateCredentials()
    {
      bool ret = false;
      try {
        var options = new DbContextOptionsBuilder<DuruDbContext>()
          .UseSqlServer("name=HotelDatabaseConfiguration")
          .Options;

        var db = new DuruDbContext(options);

        // NOTE: Not using password here, but in production you would. I intentionally leave it out so as not having to go into hashing and securing your password.
        if (db.Employees.Where(u => u.Email == Entity.Email).Count() > 0) {
          ret = true;
        }
        else {
          AddValidationMessage("LoginFailed",
                          "Invalid E-Mail and/or Password.");
        }
      }
      catch (Exception ex) {
        PublishException(ex);
      }

      return ret;
    }

    public bool Login()
    {
      bool ret = false;

      if (Validate()) {
        // Check Credentials in User Table
        if (ValidateCredentials()) {
          // Mark as logged in
          Entity.IsLoggedIn = true;

          // Send message that login was successful
          MessageBroker.Instance.SendMessage(MessageBrokerMessages.LOGIN_SUCCESS, Entity);

          // Close the user control
          Close(false);

          ret = true;
        }
      }

      return ret;
    }

    public override void Close(bool wasCancelled = true)
    {
      if (wasCancelled) {
        // Display Informational Message
        MessageBroker.Instance.SendMessage(
            MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE,
              "User NOT Logged In.");
      }

      base.Close(wasCancelled);
    }
  }
}
