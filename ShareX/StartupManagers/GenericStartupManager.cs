﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2017 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Microsoft.Win32;
using ShareX.HelpersLib;
using System;
using System.Linq;

namespace ShareX.StartupManagers
{
    public abstract class GenericStartupManager : IStartupManager
    {
        private byte[] startupEnabled = { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public abstract string StartupTargetPath { get; }

        public StartupTaskState State
        {
            get
            {
                var status = (byte[])Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder", "ShareX.lnk", startupEnabled);
                if (!status.SequenceEqual(startupEnabled))
                {
                    return StartupTaskState.DisabledByUser;
                }
                else
                {
                    return ShortcutHelpers.CheckShortcut(Environment.SpecialFolder.Startup, StartupTargetPath) ? StartupTaskState.Enabled : StartupTaskState.Disabled;
                }
            }
            set
            {
                if (value == StartupTaskState.Enabled || value == StartupTaskState.Disabled)
                {
                    ShortcutHelpers.SetShortcut(value == StartupTaskState.Enabled, Environment.SpecialFolder.Startup, StartupTargetPath, "-silent");
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}