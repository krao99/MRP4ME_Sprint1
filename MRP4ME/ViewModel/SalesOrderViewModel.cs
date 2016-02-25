using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRP4ME.Data;
using MRP4ME.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using ValidationToolkit;
using System.Diagnostics;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Threading;

namespace MRP4ME.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for a Customer object.
    /// </summary>
    public class SalesOrderViewModel : WorkspaceViewModel, INotifyDataErrorInfo
    {
        // Constraints
        public const string Constraint_Mandatory = "IsMandatory";
        public const string Constraint_MustBeNonNegative = "NonNegative";
        public const string Constraint_ValidatePositiveInteger = "ValidatePositiveInteger";
        public const string Constraint_ValidateDecimal = "ValidateDecimal";
        public const string Constraint_InvalidDate = "InvalidDate";
        
        #region Fields

        RelayCommand _saveCommand;
        RelayCommand _browseAttachmentCommand;
        RelayCommand _browseImageCommand;
        SalesOrder salesorder = new SalesOrder();
        bool isNewSO = true;

        
        #endregion // Fields

        #region Constructor

        public SalesOrderViewModel(string SONumber)
        {
            base.ErrorsChanged += OnErrorsChanged;
            this.ThisIsEnabled = true;
            if (!String.IsNullOrEmpty(SONumber))
            {
                isNewSO = false;
                PopulateSalesOrder(SONumber);
            }
            else
            {
               // this.salesorder = new SalesOrder();
                this.RequiredDate = DateTime.Today;
            }
        }

        #endregion // Constructor

        #region Sales Order Properties

        [Required(AllowEmptyStrings = false)]
        public string CustomerName
        {
            get { return salesorder.customer_name; }
            set
            {
                if (value == salesorder.customer_name)
                    return;

                salesorder.customer_name = value;
                ValidateProperty("CustomerName");
                NotifyPropertyChanged("CustomerName");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public DateTime RequiredDate
        {
            get {
                return salesorder.required_date; 
            }
            set
            {
                if (value == salesorder.required_date)
                    return;

                salesorder.required_date = value;
                ValidateProperty("RequiredDate");
                NotifyPropertyChanged("RequiredDate");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string SONumber
        {
            get { return salesorder.so_number; }
            set
            {
                if (salesorder.so_number == value)
                    return;

                salesorder.so_number = value;
                ValidateProperty("SONumber");
                NotifyPropertyChanged("SONumber");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string ItemCode
        {
            get { return salesorder.item_code; }
            set
            {
                if (value == salesorder.item_code)
                    return;

                salesorder.item_code = value;
                ValidateProperty("ItemCode");
                NotifyPropertyChanged("ItemCode");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get { return salesorder.name; }
            set
            {
                if (value == salesorder.name)
                    return;

                salesorder.name = value;
                ValidateProperty("Name");
                NotifyPropertyChanged("Name");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string Unit
        {
            get { return salesorder.unit; }
            set
            {
                if (value == salesorder.unit)
                    return;

                salesorder.unit = value;
                ValidateProperty("Unit");
                NotifyPropertyChanged("Unit");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string UnitCost
        {
            get {
                return salesorder.unit_cost; 
            }
            set
            {
                if (value == salesorder.unit_cost)
                    return;

                salesorder.unit_cost = value;
                ValidateProperty("UnitCost");
                NotifyPropertyChanged("UnitCost");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string Description
        {
            get { return salesorder.description; }
            set
            {
                if (value == salesorder.description)
                    return;

                salesorder.description = value;
                ValidateProperty("Description");
                NotifyPropertyChanged("Description");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string Quantity
        {
            get { return salesorder.quantity; }
            set
            {
                if (salesorder.quantity == value)
                    return;

                salesorder.quantity = value;
                ValidateProperty("Quantity");
                NotifyPropertyChanged("Quantity");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string QuantityReceived
        {
            get { return salesorder.quantity_received; }
            set
            {
                if (salesorder.quantity_received == value)
                    return;

                salesorder.quantity_received = value;
                ValidateProperty("QuantityReceived");
                NotifyPropertyChanged("QuantityReceived");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string BackOrdered
        {
            get { return salesorder.back_ordered; }
            set
            {
                if (salesorder.back_ordered == value)
                    return;

                salesorder.back_ordered = value;
                ValidateProperty("BackOrdered");
                NotifyPropertyChanged("BackOrdered");
            }
        }

        [StringLength(255)]
        public string Attachment
        {
            get { return salesorder.attachment; }
            set
            {
                if (value == salesorder.attachment)
                    return;

                salesorder.attachment = value;

                NotifyPropertyChanged("Attachment");
            }
        }

        [StringLength(255)]
        public string UploadImage
        {
            get { return salesorder.upload_image; }
            set
            {
                if (value == salesorder.upload_image)
                    return;

                salesorder.upload_image = value;
                NotifyPropertyChanged("UploadImage");
            }
        }

        [Required(AllowEmptyStrings = false)]
        [StringLength(2)]
        public string User
        {
            get { return salesorder.user; }
            set
            {
                if (value == salesorder.user)
                    return;

                salesorder.user = value;
                ValidateProperty("User");
                NotifyPropertyChanged("User");
            }
        }

        public string Level
        {
            get { return "0"; }
            set
            {
                if (salesorder.level == value)
                    return;

                salesorder.level = "0"; 
            }
        }

        #endregion // Sales Order Properties

        #region Presentation Properties
        bool _thisIsEnabled;
        public bool ThisIsEnabled
        {
            get { return _thisIsEnabled; }

            set
            {
                if (_thisIsEnabled == value)
                {
                    return;
                }

                _thisIsEnabled = value;
                NotifyPropertyChanged("ThisIsEnabled");
            }
        }

        /// <summary>
        /// Returns a command that saves the sales order.
        /// </summary>
        public ICommand SavePOCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand((object z) =>
                    {
                        try
                        {
                            // Shouldn't get to here if the controls do not have valid values.
                            //int x = Convert.ToInt32(this.Quantity);
                            Save();
                            
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    },
                    CanExecute);
                }
                return _saveCommand;
            }
        }

        /// <summary>
        /// Returns a command that opens file dialog.
        /// </summary>
        public ICommand BrowseAttachmentCommand
        {
            get
            {
                if (_browseAttachmentCommand == null)
                {
                    _browseAttachmentCommand = new RelayCommand(
                        param => this.OpenFileDialogForAttachment()

                        );
                }
                return _browseAttachmentCommand;
            }
        }

        /// <summary>
        /// Returns a command that opens file dialog.
        /// </summary>
        public ICommand BrowseImageCommand
        {
            get
            {
                if (_browseImageCommand == null)
                {
                    _browseImageCommand = new RelayCommand(
                        param => this.OpenFileDialogForImage()

                        );
                }
                return _browseImageCommand;
            }
        }

        #endregion Presentation Properties

        public SalesOrder SO
        {
            get { return new SalesOrder(); }
            set
            {
                if (value == salesorder)
                    return;

                salesorder = value;
            }
        }

        #region Public Methods

        private void PopulateSalesOrder(string SONumber)
        {
            sales_order _sales_order = new sales_order();

            using (var dbContext = new MRP4MEEntities())
            {
                _sales_order = dbContext.sales_order.Find(SONumber);
            
                if (!String.IsNullOrEmpty(_sales_order.so_number))
                {
                    this.ThisIsEnabled = false;
                    this.CustomerName = _sales_order.customer_name;
                    this.RequiredDate = _sales_order.required_date;
                    this.SONumber = _sales_order.so_number;
                    this.ItemCode = _sales_order.item_code;
                    this.Name = _sales_order.name;
                    this.Unit = _sales_order.unit;
                    this.UnitCost = _sales_order.unit_cost.ToString();
                    this.Description = _sales_order.description;
                    this.Quantity = _sales_order.quantity.ToString();
                    if(_sales_order.quantity_received != null )
                        this.QuantityReceived = _sales_order.quantity_received.ToString();
                    if (_sales_order.back_ordered != null)
                        this.BackOrdered = _sales_order.back_ordered.ToString();
                    this.Attachment = _sales_order.attachment;
                    this.UploadImage = _sales_order.upload_image;
                    this.User = _sales_order.user;
                    this.Level = "0";
                }
            }
        }

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            try
            {
                
                sales_order _sales_order;

                using (var dbContext = new MRP4MEEntities())
                {
                    _sales_order = dbContext.sales_order.Find(salesorder.so_number);

                    if (_sales_order != null)
                    { //check for duplicate sales order number when new SO 
                        if (isNewSO)
                            throw new DbUpdateException();
                    }
                    else
                    { //if sales order is null then it is new sales order
                        _sales_order = new sales_order();
                    }

                    DateTime dateValue = DateTime.Parse(this.RequiredDate.ToString());
                    _sales_order.customer_name = salesorder.customer_name;
                    _sales_order.required_date = DateTime.Parse(dateValue.ToString("yyyy/MM/dd"));
                    _sales_order.so_number = salesorder.so_number;
                    _sales_order.item_code = salesorder.item_code;
                    _sales_order.name = salesorder.name;
                    _sales_order.unit = salesorder.unit;
                    _sales_order.unit_cost = Convert.ToDecimal(salesorder.unit_cost);
                    _sales_order.description = salesorder.description;
                    _sales_order.quantity = Int32.Parse(salesorder.quantity);
                    if (!String.IsNullOrEmpty(salesorder.quantity_received))
                        _sales_order.quantity_received = Int32.Parse(salesorder.quantity_received);
                    if (!String.IsNullOrEmpty(salesorder.back_ordered))
                        _sales_order.back_ordered = Int32.Parse(salesorder.back_ordered);
                    _sales_order.attachment = salesorder.attachment;
                    _sales_order.upload_image = salesorder.upload_image;
                    _sales_order.user = salesorder.user;
                    _sales_order.level = 0;

                    if (isNewSO)
                    {
                        dbContext.sales_order.Add(_sales_order);
                        dbContext.SaveChanges();
                        dbContext.Entry(_sales_order).State = EntityState.Detached;
                    }
                    else
                    {
                        dbContext.SaveChanges();
                        dbContext.Entry(_sales_order).State = EntityState.Modified;
                        dbContext.Entry(_sales_order).State = EntityState.Detached;
                    }
                }
                ClearForm();
               
                MessageBox.Show("Sales Order successfully saved.", "MRP4ME");
                //NotifyPropertyChanged("SO");
            }
            catch (DbUpdateException ex)
            {
                //MessageBox.Show("SO Number already exist!" , "MRP4ME");
                AddError(new ValidationError("SONumber", Constraint_Mandatory, " SO Number already exist!"));
                NotifyPropertyChanged("SONumber");
                Tracer.LogUserDefinedValidation("DbUpdateException. " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sales Order not saved, please verify data and save again." + ex.Message, "MRP4ME");
            }
        }

        private void ClearForm()
        {
            this.CustomerName = "";
            this.RequiredDate = DateTime.Today;
            this.SONumber = "";
            this.ItemCode = "";
            this.Name = "";
            this.Unit = "";
            this.UnitCost = "";
            this.Description = "";
            this.Quantity = "";
            this.QuantityReceived = "";
            this.BackOrdered = "";
            this.Attachment = "";
            this.UploadImage = "";
            this.User = "";
            this.Level = "0";
            base.errors.Clear();
            base.ErrorsChanged += OnErrorsChanged;
            NotifyPropertyChanged("CustomerName");
            NotifyPropertyChanged("SONumber");
            NotifyPropertyChanged("ItemCode");
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Unit");
            NotifyPropertyChanged("UnitCost");
            NotifyPropertyChanged("Description");
            NotifyPropertyChanged("Quantity");
            NotifyPropertyChanged("User");
            
        }
        
        private void OpenFileDialogForAttachment()
        {
            Microsoft.Win32.OpenFileDialog fileDlg = new Microsoft.Win32.OpenFileDialog();
            fileDlg.Filter = "All files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = fileDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                this.Attachment = fileDlg.FileName;
            }
            else
            {
                this.Attachment = "";
            }
        }

        private void OpenFileDialogForImage()
        {
            Microsoft.Win32.OpenFileDialog fileDlg = new Microsoft.Win32.OpenFileDialog();
            fileDlg.Filter = "All files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = fileDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                this.UploadImage = fileDlg.FileName;
            }
            else
            {
                this.UploadImage = "";
            }
        }

        #endregion // Public Methods

        #region Private Helpers

        public bool CanExecute(object z)
        {
            //int num1;
            return isSOValid()
                   && ErrorCount == 0;
        }

        public bool isSOValid()
        {
            bool validSO = true;

            if (String.IsNullOrEmpty(this.CustomerName))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.RequiredDate.ToString()))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.SONumber))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.ItemCode))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.Name))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.Unit))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.UnitCost))
            {
                validSO = false;
            }
            else if (String.IsNullOrEmpty(this.Description))
            {
                validSO = false;
            } 
            else if(String.IsNullOrEmpty(this.Quantity)) {
                validSO = false;
            } 
            else if (String.IsNullOrEmpty(this.User))
            {
                validSO = false;
            } 
            return validSO;
        }
         
        #endregion // Private Helpers

        #region validation

        private void ValidatePositiveInteger(string x, string fieldName)
        {
            long num1;
            RemoveError(fieldName, Constraint_ValidatePositiveInteger);

           if (String.IsNullOrEmpty(x))
            {
                AddError(new ValidationError(fieldName, Constraint_ValidatePositiveInteger, " is required."));
            }
            else if (!long.TryParse(x, out num1))
            {
                AddError(new ValidationError(fieldName, Constraint_ValidatePositiveInteger, " must be number."));
            }
            else if (Convert.ToInt64(x) < 0)
            {
                AddError(new ValidationError(fieldName, Constraint_ValidatePositiveInteger, " must be non-negative."));
            }
        }

        private void ValidateDecimal(string x, string fieldName)
        {
            decimal num1;
            RemoveError(fieldName, Constraint_ValidateDecimal);
            if (String.IsNullOrEmpty(x))
            {
                AddError(new ValidationError(fieldName, Constraint_ValidateDecimal, " is required."));
            }
            else if (!decimal.TryParse(x, out num1))
            {
                AddError(new ValidationError(fieldName, Constraint_ValidateDecimal, " must be number."));
            }
            else if (Convert.ToDecimal(x) < 0)
            {
                AddError(new ValidationError(fieldName, Constraint_ValidateDecimal, " must be non-negative."));
            }
        }

        private void ValidateDate(DateTime dt, string fieldName)
        {
            DateTime temp;
            RemoveError(fieldName, Constraint_InvalidDate);

            if (String.IsNullOrEmpty(dt.ToString()))
            {
                AddError(new ValidationError(fieldName, Constraint_InvalidDate, " is required."));
            }
            else if (!DateTime.TryParse(dt.ToString(), out temp))
            {
                AddError(new ValidationError(fieldName, Constraint_InvalidDate, " must be a valid date."));
            }
        }

        private void ValidateEmptyString(string x, string fieldName)
        {
            RemoveError(fieldName, Constraint_Mandatory);

            if (String.IsNullOrEmpty(x))
            {
                AddError(new ValidationError(fieldName, Constraint_Mandatory, " is required."));
            }
        }

        public void ValidateProperty(string propertyName)
        {
            Tracer.LogValidation("INotifyDataErrorInfo.ValidateProperty called. Validating " + propertyName);

            switch (propertyName)
            {
                case "CustomerName":
                    {
                        ValidateEmptyString(this.CustomerName, propertyName);
                    }
                    break;
                case "RequiredDate":
                    {
                        ValidateDate(this.RequiredDate, propertyName);
                    }
                    break;
                case "SONumber":
                    {
                        ValidateEmptyString(this.SONumber, propertyName);
                    }
                    break;
                case "ItemCode":
                    {
                        ValidateEmptyString(this.ItemCode, propertyName);
                    }
                    break;
                case "Name":
                    {
                        ValidateEmptyString(this.Name, propertyName);
                    }
                    break;
                case "Unit":
                    {
                        ValidateEmptyString(this.Unit, propertyName);
                    }
                    break;
                case "UnitCost":
                    {
                        ValidateDecimal(this.UnitCost, propertyName);
                    }
                    break;
                case "Quantity":
                    {
                        ValidatePositiveInteger(this.Quantity, propertyName);
                    }
                    break;
                case "QuantityReceived":
                    {
                        if (!String.IsNullOrEmpty(this.QuantityReceived))
                            ValidatePositiveInteger(this.QuantityReceived, propertyName);
                        else
                            RemoveError("QuantityReceived", Constraint_ValidatePositiveInteger);
                    }
                    break;
                case "BackOrdered":
                    {
                        if (!String.IsNullOrEmpty(this.BackOrdered))
                            ValidatePositiveInteger(this.BackOrdered, propertyName);
                        else
                            RemoveError("BackOrdered", Constraint_ValidatePositiveInteger);
                    }
                    break;
                case "User":
                    {
                        ValidateEmptyString(this.User, propertyName);
                    }
                    break;
            }
            if (String.IsNullOrEmpty(propertyName))
            {
                Tracer.LogValidation("No cross-property validation errors.");
            }
        }
        
        #endregion validation

        #region INotifyErrorDataInfo

        // INotifyErrorDataInfo.
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return base.GetPropertyErrors(propertyName);
        }

        // INotifyErrorDataInfo.
        public bool HasErrors
        {
            get { return ErrorCount != 0; }
        }

        // Helper
        private void RaiseErrorsChanged(string propertyName)
        {
            NotifyErrorsChanged(propertyName);
        }
        #endregion

        #region CurrentValidationError

        // The "CurrentValidationError" property is kept up-to-date with the "latest" error.
        public void OnErrorsChanged(object sender, DataErrorsChangedEventArgs args)
        {
            string propertyName = args.PropertyName;
            Tracer.LogUserDefinedValidation("OnErrorsChanged called. " + GetValidationErrorMessagesAsString());
            NotifyPropertyChanged("CurrentValidationError");
        }

        // Bind target for error bar.
        public virtual ValidationError CurrentValidationError
        {
            get
            {
                if (ErrorCount == 0)
                    return null;

                // Get the error list associated with the last property to be validated.
                Debug.Assert(!String.IsNullOrEmpty(lastPropertyValidated));
                List<ValidationError>.Enumerator p = errors[lastPropertyValidated].GetEnumerator();

                // Decide which error needs to be returned.
                ValidationError error = null;
                while (p.MoveNext())
                {
                    error = p.Current;
                    if (error.ID == "System.Windows.Controls.ExceptionValidationRule")
                        break;
                }
                return error;
            }
        }
        #endregion INotifyErrorDataInfo
    }
}
