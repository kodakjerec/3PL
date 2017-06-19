using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;

namespace _3PL_LIB
{
    public class ControlBind
    {
        /// <summary>
        /// DropDownList共用元件,如果不需要添加第一顯示項目,則不需輸入Str_TopItem與Str_TopItemValue
        /// </summary>
        /// <param name="DDL">要bind的元件</param>
        /// <param name="dt">要bind的source</param>
        /// <param name="Str_ValueField">ValueField的欄位名稱</param>
        /// <param name="Str_TextField">TextField的欄位名稱</param>
        /// <param name="Str_TopItem">第一個顯示項目的text</param>
        /// <param name="Str_TopItemValue">第一個顯示項目text的值</param>
        public void DropDownListBind(ref  DropDownList DDL, DataTable dt, string Str_ValueField, string Str_TextField, string Str_TopItem, string Str_TopItemValue)
        {
            try
            {
                if (DDL == null) return;
                DDL.Items.Clear();
                DDL.DataSource = dt;
                DDL.DataValueField = Str_ValueField;
                DDL.DataTextField = Str_TextField;
                DDL.DataBind();
                if (Str_TopItem == null || Str_TopItemValue == null)
                    return;
                DDL.Items.Insert(0, new ListItem(Str_TopItem, Str_TopItemValue));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// DropDownList共用元件,如果不需要添加第一顯示項目,則不需輸入Str_TopItem與Str_TopItemValue
        /// </summary>
        /// <param name="DDL">要bind的元件</param>
        /// <param name="dt">要bind的source</param>
        /// <param name="Str_ValueField">ValueField的欄位名稱</param>
        /// <param name="Str_TextField">TextField的欄位名稱</param>
        public void DropDownListBind(ref  DropDownList DDL, DataTable dt, string Str_ValueField, string Str_TextField)
        {
            try
            {
                if (DDL == null) return;
                DDL.Items.Clear();
                DDL.DataSource = dt;
                DDL.DataValueField = Str_ValueField;
                DDL.DataTextField = Str_TextField;
                DDL.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// RadioButtonList共用元件
        /// </summary>
        /// <param name="RBL">要bind的元件</param>
        /// <param name="dt">要bind的source</param>
        /// <param name="Str_ValueField">ValueField的欄位名稱</param>
        /// <param name="Str_TextField">TextField的欄位名稱</param>
        public void RadioBtnListBind(ref RadioButtonList RBL, DataTable dt, string Str_ValueField, string Str_TextField)
        {
            try
            {
                if (RBL == null) return;
                RBL.Items.Clear();
                RBL.DataSource = dt;
                RBL.DataValueField = Str_ValueField;
                RBL.DataTextField = Str_TextField;
                RBL.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// CheckBoxList共用元件
        /// </summary>
        /// <param name="CBL">要bind的元件</param>
        /// <param name="dt">要bind的source</param>
        /// <param name="Str_ValueField">ValueField的欄位名稱</param>
        /// <param name="Str_TextField">TextField的欄位名稱</param>
        public void CheckBoxListBind(ref CheckBoxList CBL, DataTable dt, string Str_ValueField, string Str_TextField)
        {
            try
            {
                if (CBL == null) return;
                CBL.Items.Clear();
                CBL.DataSource = dt;
                CBL.DataValueField = Str_ValueField;
                CBL.DataTextField = Str_TextField;
                CBL.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
