﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MEL
namespace MVCVisualDesigner
{
    public partial class VDViewBase
    {
        internal string GetViewModelTypeValue()
        {
            if (this.Model != null)
            {
                return this.Model.FullName;
            }
            return string.Empty;
        }
    }

    public partial class VDView
    {
        public WidgetInfoManager WidgetInfoManager { get; set; }
    }

    public partial class VDPartialView
    {
        public override bool HasWidgetTitle { get { return true; } }
    }
}


// PEL
namespace MVCVisualDesigner
{
    public partial class VDPartialViewShape
    {
        protected override System.Drawing.Image getTitleIcon()
        {
            return this.getImageFromResource("PartialViewToolToolboxBitmap");
        }

        protected override string getTitleText()
        {
            VDPartialView view = this.GetMEL<VDPartialView>();
            if (view != null && !string.IsNullOrEmpty(view.WidgetName))
                return string.Format("{0} - {1}  ", base.getTitleText(), view.WidgetName);
            else
                return base.getTitleText();
        }
    }
}
