using System;
using System.Collections;
using System.Web.UI;

namespace Hd.Web.Extensions
{
    public class ControlFinder
    {
        public static Control[] FindControls(Control targetControl, Type controlsToFindType)
        {
            return FindControls(targetControl, controlsToFindType, false);
        }

        public static Control[] FindControls(Control targetControl, Type controlsToFindType, bool stringTypeNameCompare)
        {
            ArrayList arrayList = new ArrayList();
            ArrayList tmpControls = new ArrayList(targetControl.Controls);
            tmpControls.Reverse();

            Stack stack = new Stack(tmpControls);

            while (stack.Count > 0)
            {
                Control control = stack.Pop() as Control;

                if (stringTypeNameCompare)
                {
                    if (control.GetType() == controlsToFindType)
                        arrayList.Add(control);
                }
                else
                {
                    if (control.GetType() == controlsToFindType)
                    {
                        arrayList.Add(control);
                    }
                    else if (controlsToFindType.IsInstanceOfType(control) ||
                             control.GetType().IsSubclassOf(controlsToFindType))
                    {
                        arrayList.Add(control);
                    }
                }

                foreach (Control ctrl in control.Controls)
                    stack.Push(ctrl);
            }

            Control[] controls = new Control[arrayList.Count];
            arrayList.CopyTo(controls);
            return controls;
        }

        public static Control FindFirstControl(Control targetControl, Type controlsToFindType)
        {
            Stack stack = new Stack(targetControl.Controls);

            while (stack.Count > 0)
            {
                Control control = stack.Pop() as Control;

                if (control.GetType() == controlsToFindType)
                    return control;

                foreach (Control childControl in control.Controls)
                {
                    if (childControl.GetType() == controlsToFindType)
                        return childControl;
                    else if (childControl.Controls.Count > 0)
                        stack.Push(childControl);
                }
            }

            return null;
        }


        public static Control[] FindInterfaceImplementers(Control targetControl, Type interfaceToMatch)
        {
            ArrayList arrayList = new ArrayList();


            Stack stack = new Stack(targetControl.Controls);

            while (stack.Count > 0)
            {
                Control control = stack.Pop() as Control;

                if (control.GetType().GetInterface(interfaceToMatch.Name) != null)
                    arrayList.Add(control);

                foreach (Control ctrl in control.Controls)
                    stack.Push(ctrl);
            }

            Control[] controls = new Control[arrayList.Count];
            arrayList.CopyTo(controls);
            return controls;
        }

        public static Control FindFirstParentControl(Control targetControl, Type controlsToFindType)
        {
            Control parent = targetControl.Parent;
            bool isParentExists = targetControl.Parent != null;
            
            
            while(isParentExists)
            {
                if (parent.GetType() == controlsToFindType)
                    return parent;
                
                isParentExists = parent.Parent != null;
                parent = parent.Parent;
            }
            
            return parent;
        }

        public static Control FindFirstControl(Control targetControl, string controlID)
        {
            Stack stack = new Stack(targetControl.Controls);

            while (stack.Count > 0)
            {
                Control control = stack.Pop() as Control;

                if (control.ID != null && control.ID.ToLower() == controlID.ToLower())
                    return control;

                foreach (Control ctrl in control.Controls)
                    stack.Push(ctrl);
            }

            return null;
        }
    }
}