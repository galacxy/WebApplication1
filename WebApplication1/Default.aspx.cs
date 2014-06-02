using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using Search;

namespace WebApplication1
{
    public partial class search : System.Web.UI.Page
    {
        String message;
        KeyValuePair<String, String>[] Results;
        Int32 numResults;
        Int32 lower;
        KeyValuePair<String, String>[] CurrentWindow;
        Int32 windowSize;

        public Int32 WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }

        public KeyValuePair<String, String>[] CurrentWindow1
        {
            get { return CurrentWindow; }
            set { CurrentWindow = value; }
        }

        public Int32 Lower
        {
            get { return lower; }
            set { lower = value; }
        }
        Int32 upper;

        public Int32 Upper
        {
            get { return upper; }
            set { upper = value; }
        }

        public Int32 NumResults
        {
            get { return numResults; }
            set { numResults = value; }
        }

        public KeyValuePair<String, String>[] Results1
        {
            get { return Results; }
            set { Results = value; }
        }

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

        public search()
        {
            Lower = 0;
            windowSize = Upper = 15;
            CurrentWindow1 = new KeyValuePair<string, string>[WindowSize];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                Label1.Text = "WikiSearch";
                Label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
                Label1.Font.Bold = true;
                Label2.Text = "";
                Label2.Font.Bold = true;
                TextBox1.BorderColor = System.Drawing.Color.LightGray;
                repLinks.Visible = false;
                Label1.TabIndex = 0;
                //Literal lit1 = new Literal();
                //lit1.Text = @"<span class='glyphicon glyphicon-search'></span> ";
                //Button1.Text = lit1.Text;
                LinkButton1.Visible = false;
                LinkButton2.Visible = false;
            }
            TextBox1.Focus();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            //Session.Clear();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (TextBox1.Text == "")
                {
                    Message = "Please fill out the field";
                    Label2.ForeColor = System.Drawing.Color.Red;
                    TextBox1.BorderColor = System.Drawing.Color.Crimson;
                }
                else
                {
                    TextBox1.BorderColor = System.Drawing.Color.LightGray;
                    Label2.ForeColor = System.Drawing.Color.Green;
                    Session.Clear();
                    String filename = @"D:\rohit.bansal\SandBox\Interns\rohit.bansal\WebApplication1\WebApplication1\mergerResults.csv";
                    ExecuteSearch ES = new ExecuteSearch(filename);
                    ES.getSearchResults(TextBox1.Text, out Results);
                    if (Results == null)
                    {
                        Label2.ForeColor = System.Drawing.Color.Red;
                        repLinks.Visible = false;
                        LinkButton1.Visible = false;
                        LinkButton2.Visible = false;
                        Message = "Unable to load indexfile";
                    }
                    else
                    {
                        NumResults = Results1.Length;

                        if (NumResults > 0)
                        {
                            for (int index = Lower; index < WindowSize; index++)
                            {
                                try
                                {
                                    CurrentWindow1[index] = new KeyValuePair<string, string>(Results1[index].Key, Results1[index].Value);
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            repLinks.DataSource = CurrentWindow1;
                            repLinks.DataBind();
                            repLinks.Visible = true;
                            LinkButton1.Visible = false;
                            LinkButton2.Visible = NumResults > WindowSize ? true : false;
                            Upper = NumResults > windowSize ? Upper : NumResults;
                            Session.Add("WikiSearch_numResult", NumResults);
                            Session.Add("WikiSearch_lower", Lower);
                            Session.Add("WikiSearch_upper", Upper);
                            Session.Add("WikiSearchResults", Results1);
                            Message = "About " + NearestNum(NumResults) + " results found";
                        }
                        else
                        {
                            Label2.ForeColor = System.Drawing.Color.Red;
                            Message = "No results found. Check your spellings and try again";
                            repLinks.Visible = false;
                            LinkButton1.Visible = false;
                            LinkButton2.Visible = false;
                        }
                    }
                }
                Label2.Text = Message;
            }
        }

        private Int64 NearestNum(Int64 Number)
        {
            int temp = 10;
            while ((Number/temp) > 0)
            {
                temp *= 10;
            }
            temp /= 1000;
            try
            {
                return Number - (Number % temp);
            }
            catch
            {
                return Number - (Number % 10);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            RetrieveFromSession(out Results, out numResults, out lower, out upper);
            Upper -= WindowSize;
            Lower -= WindowSize;
            if (lower == 0)
            {
                LinkButton1.Visible = false;
            }
            Session["WikiSearch_numResult"] = NumResults;
            Session["WikiSearch_lower"] = Lower;
            Session["WikiSearch_upper"] = Upper;
            LinkButton2.Visible = true;
            Message = "Showing " + Lower + " to " + Upper + " of " + NumResults + " results found";
            Int32 GlobalIndex = Lower;
            CurrentWindow1 = null;
            CurrentWindow1 = new KeyValuePair<string, string>[WindowSize];
            for (int index = 0; index < WindowSize; index++)
            {
                try
                {
                    CurrentWindow1[index] = new KeyValuePair<string, string>(Results1[GlobalIndex].Key, Results1[GlobalIndex].Value);
                    GlobalIndex++;
                }
                catch (Exception exp)
                {
                    Message = exp.Message;
                    break;
                }
            }
            repLinks.DataSource = CurrentWindow1;
            repLinks.DataBind();
            Label2.Text = Message;
        }

        protected void RetrieveFromSession(out KeyValuePair<String, String>[] results, out Int32 numResult, out Int32 lower, out Int32 upper)
        {
            try
            {
                numResult = (Int32)Session["WikiSearch_numResult"];
                lower = (Int32)Session["WikiSearch_lower"];
                upper = (Int32)Session["WikiSearch_upper"];
                results = new KeyValuePair<string, string>[numResult];
                results = (KeyValuePair<string, string>[])Session["WikiSearchResults"];
            }
            catch
            {
                numResult = upper = lower = 0;
                results = null;
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            RetrieveFromSession(out Results, out numResults, out lower, out upper);
            Upper = Upper + WindowSize;
            Lower = Lower + WindowSize;
            if (Upper > NumResults)
            {
                Upper = NumResults;
                LinkButton2.Visible = false;
            }
            LinkButton1.Visible = true;
            Session["WikiSearch_numResult"] = NumResults;
            Session["WikiSearch_lower"] = Lower;
            Session["WikiSearch_upper"] = Upper;
            Message = "Showing " + Lower + " to " + Upper + " of " + NumResults + " results found";
            Int32 GlobalIndex = Lower;
            CurrentWindow1 = null;
            CurrentWindow1 = new KeyValuePair<string, string>[WindowSize];
            for (int index = 0; index < WindowSize; index++)
            {
                try
                {
                    CurrentWindow1[index] = new KeyValuePair<string, string>(Results1[GlobalIndex].Key, Results1[GlobalIndex].Value);
                    //CurrentWindow1[index].Key = Results1[GlobalIndex].Key;
                    //CurrentWindow1[index].Value = Results1[GlobalIndex].Value;
                    GlobalIndex++;
                }
                catch (Exception exp)
                {
                    Message = exp.Message;
                    break;
                }
            }
            repLinks.DataSource = CurrentWindow1;
            repLinks.DataBind();
            Label2.Text = Message;
        }


    }
}