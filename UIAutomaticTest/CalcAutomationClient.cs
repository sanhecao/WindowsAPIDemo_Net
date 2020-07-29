using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIAutomaticTest
{
    class CalcAutomationClient
    {
        AutomationElement calWindow = null;//计算器窗口主窗口元素


        string resultTextAutoID = "CalculatorResults";
        string btn5AutoID = "num5Button";
        string btn3AutoID = "num3Button";
        string btn2AutoID = "num2Button";
        string btnPlusAutoID = "plusButton";
        string btnSubAutoId = "94";
        string btnEqualAutoID = "equalButton";
        static void Main(string[] args)
        {
            CalcAutomationClient autoClient = new CalcAutomationClient();
            AutomationEventHandler eventHandler = new AutomationEventHandler(autoClient.OnWindowOpenOrClose);
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, eventHandler);
            Process.Start("calc.exe");
            Console.ReadLine();
        }
        private void OnWindowOpenOrClose(object sender, AutomationEventArgs e)
        {
            if (calWindow != null)
                return;
            if (e.EventId != WindowPattern.WindowOpenedEvent)
            {
                return;
            }
            if (sender == null)
            {
                Console.WriteLine("sender is null");
                return;
            }
            Thread.Sleep(1000);//此处必须等待一下，应该是计算器的等待计算器完全加载，不然控件 找不到
            AutomationElement sourceElement = null;
            sourceElement = sender as AutomationElement;
            Console.WriteLine(sourceElement.Current.Name);
            try
            {
                sourceElement = sender as AutomationElement;
                Console.WriteLine(sourceElement.Current.Name);
                if (sourceElement.Current.Name == "计算器")
                {
                    calWindow = sourceElement;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
                return;
            }
            if (calWindow == null)
            {
                return;
            }
            ExcuteTest();
        }
        private void ExcuteTest()
        {
            ExcuteButtonInvoke(btn2AutoID);
            ExcuteButtonInvoke(btnPlusAutoID);
            ExcuteButtonInvoke(btn3AutoID);
            ExcuteButtonInvoke(btnEqualAutoID);
            string rs = GetCurrentResult();
            Console.WriteLine(rs);
        }
        private void ExcuteButtonInvoke(string automationId)
        {
            Condition conditions = new AndCondition(
              new PropertyCondition(AutomationElement.AutomationIdProperty, automationId),
              new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
            if (calWindow == null)
                return;
            AutomationElementCollection collection = calWindow.FindAll(TreeScope.Descendants, conditions);
            if (collection == null || collection.Count == 0)
                return;
            AutomationElement btn = collection[0];
            if (btn != null)
            {
                InvokePattern invokeptn = (InvokePattern)btn.GetCurrentPattern(InvokePattern.Pattern);
                invokeptn.Invoke();
            }
            Thread.Sleep(1000);
        }
        private string GetCurrentResult()
        {
            Condition conditions = new AndCondition(
              new PropertyCondition(AutomationElement.AutomationIdProperty, resultTextAutoID),
              new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
            AutomationElement text = calWindow.FindAll(TreeScope.Descendants, conditions)[0];
            return text.Current.Name;
        }


    }
}
