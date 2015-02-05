﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Web.UI;
using MixERP.Net.Common;
using MixERP.Net.Common.Base;
using MixERP.Net.Common.Domains;
using MixERP.Net.Common.Helpers;
using Serilog;

namespace MixERP.Net.FrontEnd.Base
{
    public abstract class MixERPUserControl : MixERPUserControlBase
    {
        public override AccessLevel AccessLevel
        {
            get { return AccessLevel.PolicyBased; }
        }

        public void Initialize()
        {
            Log.Verbose("{Control} initialized.", this);
            this.OnControlLoad(this, new EventArgs());

            this.CheckAccessLevel();
        }

        private void CheckAccessLevel()
        {
            if ((this.AccessLevel.Equals(AccessLevel.AdminOnly) || this.AccessLevel.Equals(AccessLevel.LocalhostAdmin)) && !CurrentSession.IsAdmin())
            {
                Log.Information("Access to {Control} is denied to user.", this, CurrentSession.GetUserName());

                this.Page.Server.Transfer("~/Site/AccessIsDenied.aspx");
            }

            bool isLocalHost = PageUtility.IsLocalhost(this.Page);

            if (this.AccessLevel.Equals(AccessLevel.LocalhostAdmin) && !isLocalHost)
            {
                Log.Information("Access to {Control} is denied to user.", this, CurrentSession.GetUserName());

                this.Page.Server.Transfer("~/Site/AccessIsDenied.aspx");
            }
        }
    }
}