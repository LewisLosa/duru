using System;
using System.Collections.ObjectModel;
using Common.Library;
using Duru.Library.Exceptions;
using Duru.Library.Validation;

namespace Duru.Library.ViewModels
{
    public class ViewModelBase : CommonBase
    {
        #region Private Variables
        private ObservableCollection<ValidationMessage> _ValidationMessages = new ObservableCollection<ValidationMessage>();
        private bool _IsValidationVisible = false;
        #endregion

        #region Public Properties
        public ObservableCollection<ValidationMessage> ValidationMessages
        {
            get { return _ValidationMessages; }
            set {
                _ValidationMessages = value;
                RaisePropertyChanged("ValidationMessages");
            }
        }

        public bool IsValidationVisible
        {
            get { return _IsValidationVisible; }
            set {
                _IsValidationVisible = value;
                RaisePropertyChanged("IsValidationVisible");
            }
        }
        #endregion

        #region AddBusinessRuleMessage Method
        public virtual void AddValidationMessage(string propertyName, string msg)
        {
            _ValidationMessages.Add(new ValidationMessage { Message = msg, PropertyName = propertyName });
            IsValidationVisible = true;
        }
        #endregion

        #region Clear Method
        public virtual void Clear()
        {
            ValidationMessages.Clear();
            IsValidationVisible = false;
        }
        #endregion

        #region DisplayStatusMessage Method
        public virtual void DisplayStatusMessage(string msg = "")
        {
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.DISPLAY_STATUS_MESSAGE, msg);
        }
        #endregion

        #region PublishException Method
        public void PublishException(Exception ex)
        {
            // Publish Exception
            ExceptionManager.Instance.Publish(ex);
        }
        #endregion

        #region Close Method
        public virtual void Close(bool wasCancelled = true)
        {
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.CLOSE_USER_CONTROL, wasCancelled);
        }
        #endregion

        #region Dispose Method
        public virtual void Dispose()
        {
        }
        #endregion
    }
}