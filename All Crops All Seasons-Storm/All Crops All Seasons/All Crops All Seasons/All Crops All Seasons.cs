/*
    Copyright 2016 cantorsdust

    Storm is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Storm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Storm.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using Storm.ExternalEvent;
using Storm.StardewValley.Event;
using Storm.StardewValley.Proxy;
using Storm.StardewValley.Wrapper;
using System.Collections.Generic;
using Storm;
using Storm.Collections;
using Storm.StardewValley.Accessor;

namespace AllCropsAllSeasons
{
    [Mod]
    public class AllCropsAllSeasons : DiskResource
    {
        //public static ModConfig ACASConfig { get; private set; }
        public string oldname;
        /*
        private class CustomSeeds : ObjectDelegate
        {
            public int parentSpriteSheetIndex;
            public int initialStack;

            public CustomSeeds(int parentSpriteSheetIndex, int initialStack)
            {
                this.parentSpriteSheetIndex = parentSpriteSheetIndex;
                this.initialStack = initialStack;
            }

            public override object[] GetConstructorParams()
            {
                return new object[] { this.parentSpriteSheetIndex, this.initialStack, };
            }
        }
        */

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            /*
            ACASConfig = new ModConfig();
            ACASConfig = (ModConfig)Config.InitializeConfig(PathOnDisk + "\\Config.json", ACASConfig);
            */
            Console.WriteLine("ACAS Initialization Completed");
        }
        
        [Subscribe]
        public void PreDayUpdateHoeDirtCallback(PreDayUpdateHoeDirtEvent @event)
        {
            Console.WriteLine("Firing PreDayUpdateHoeDirtCallback");
            oldname = @event.Environment.Name;
            @event.Environment.Name = "Greenhouse";
        }
        
        [Subscribe]
        public void PostDayUpdateHoeDirtCallback(PostDayUpdateHoeDirtEvent @event)
        {
            Console.WriteLine("Firing PostDayUpdateHoeDirtCallback");
            @event.Environment.Name = oldname;
        }
        /*
        [Subscribe]
        public void PreCropConstructorCallback(PreCropConstructorEvent @event)
        {
            Console.WriteLine("Firing PreCropConstructorCallback");
        }
        */
        [Subscribe]
        public void PostCropConstructorCallback(PostCropConstructorEvent @event)
        {
            Console.WriteLine("Firing PostCropConstructorCallback");
            if (!@event.Crop.SeasonsToGrowIn.Contains("spring"))
            {
                @event.Crop.SeasonsToGrowIn.Add("spring");
                //Console.WriteLine("Adding spring to crop");
            }
            if (!@event.Crop.SeasonsToGrowIn.Contains("summer"))
            {
                @event.Crop.SeasonsToGrowIn.Add("summer");
                //Console.WriteLine("Adding summer to crop");
            }
            if (!@event.Crop.SeasonsToGrowIn.Contains("fall"))
            {
                @event.Crop.SeasonsToGrowIn.Add("fall");
                //Console.WriteLine("Adding fall to crop");
            }
            if (!@event.Crop.SeasonsToGrowIn.Contains("winter"))
            {
                @event.Crop.SeasonsToGrowIn.Add("winter");
                //Console.WriteLine("Adding winter to crop");
            }
        }

        [Subscribe]
        public void PreHoeDirtCanPlantCallback(PreHoeDirtCanPlantEvent @event)
        {

            Console.WriteLine("Firing PreHoeDirtCanPlantCallback");
            //oldname = @event.Root.CurrentLocation.Name;
            //@event.Root.CurrentLocation.Name = "Greenhouse";
            //@event.ReturnValue = true;
            //@event.ReturnEarly = true;

            if (@event.IsFertilizer)
            {
                if (@event.HoeDirt.Fertilizer == 0)
                {
                    @event.ReturnValue = true;
                    @event.ReturnEarly = true;
                }
            }
            else if (@event.HoeDirt.Crop == null)
            {
                @event.ReturnValue = true;
                @event.ReturnEarly = true;
            }
            else
            {
                @event.ReturnValue = false;
                @event.ReturnEarly = true;
            }



        }

        /*
        [Subscribe]
        public void PreHoeDirtPlantCallback(PreHoeDirtPlantEvent @event)
        {
            Console.WriteLine("Firing PreHoeDirtPlantCallback");
            oldname = @event.Farmer.CurrentLocation.Name;
            @event.Farmer.CurrentLocation.Name = "Greenhouse";
        }
        
        
        [Subscribe]
        public void PostHoeDirtPlantCallback(PostHoeDirtPlantEvent @event)
        {
            if (@event.Farmer.CurrentLocation.Name == "Greenhouse")
            {
                Console.WriteLine(@event.Farmer.CurrentLocation.Name);
            }

            Console.WriteLine("Firing PostHoeDirtPlantCallback");
            @event.Farmer.CurrentLocation.Name = oldname;
        }
        
       
        
        
        [Subscribe]
        public void PostHoeDirtCanPlantCallback(PostHoeDirtCanPlantEvent @event)
        {

            Console.WriteLine("Firing PostHoeDirtCanPlantCallback");
        }

        [Subscribe]
        public void PreConstructShopViaListCallback(PreConstructShopViaListEvent @event)
        {
            Console.WriteLine("Firing PreConstructShopViaListCallback");


        }

        [Subscribe]
        public void PostConstructShopViaListCallback(PostConstructShopViaListEvent @event)
        {
            Console.WriteLine("Firing PostConstructShopViaListCallback");
            //var item = @event.ProxyObject(new StandardObjectDelegate(487, 1));

        }
        /*
        [Subscribe]
        public void PreConstructShopViaDictCallback(PreConstructShopViaDictEvent @event)
        {
            Console.WriteLine("Firing PreConstructShopViaDictCallback");
            //List<> item = @event.ItemPriceAndStock.Keys;


        }

        [Subscribe]
        public void PostConstructShopViaDictCallback(PostConstructShopViaDictEvent @event)
        {
            //var wrapper = @event;
            Console.WriteLine("Firing PostConstructShopViaDictCallback");
            //@event.ItemsForSale.Add((Wrapper<ObjectItem>)new CustomSeeds(487, int.MaxValue));
            
        }
        
        [Subscribe]
        public void SetUpShopOwnerCallback(SetUpShopOwnerEvent @event)
        {
            Console.WriteLine("Firing SetUpShopOwnerCallback");
        }
        */


    }
    /*
    public class ModConfig : Config
    {
        public int TenMinuteTickLength { get; set; }
        public bool ChangeTimeSpeedOnFestivalDays { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            TenMinuteTickLength = 14;
            ChangeTimeSpeedOnFestivalDays = false;

            return this;
        }
    }
    */
}

/*
(2:52:11 AM) Demmonic: ok, then to add items
(2:52:16 AM) Demmonic: you do this
(2:52:17 AM) Demmonic: <Demmonic>  var item = @event.ProxyObject(new StandardObjectDelegate(3, 1));
(2:52:17 AM) Demmonic: <Demmonic> <Demmonic>             wrapper.ItemsForSale.Add(item);
(2:52:17 AM) Demmonic: <Demmonic> <Demmonic>             wrapper.ItemsForSaleData.Add(item, new int[] { 1, 1 });
(2:52:20 AM) Demmonic: where wrapper is the Shop
    */
