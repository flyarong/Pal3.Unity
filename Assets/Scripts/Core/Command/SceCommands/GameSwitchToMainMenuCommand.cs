﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Core.Command.SceCommands
{
    [SceCommand(147, "退出游戏到主菜单")]
    public class GameSwitchToMainMenuCommand : ICommand
    {
        public GameSwitchToMainMenuCommand() {}
    }
}