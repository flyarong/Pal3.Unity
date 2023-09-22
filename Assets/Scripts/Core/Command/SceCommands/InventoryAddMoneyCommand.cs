﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Core.Command.SceCommands
{
    [SceCommand(48, "增减金钱，" +
                    "参数：增减量")]
    public class InventoryAddMoneyCommand : ICommand
    {
        public InventoryAddMoneyCommand(int changeAmount)
        {
            ChangeAmount = changeAmount;
        }

        public int ChangeAmount { get; }
    }
}