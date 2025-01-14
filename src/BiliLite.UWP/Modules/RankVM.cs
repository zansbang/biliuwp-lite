﻿using BiliLite.Models;
using BiliLite.Models.Requests.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiliLite.Extensions;

namespace BiliLite.Modules
{
    public class RankVM : IModules
    {
        readonly RankAPI rankAPI;
        public RankVM()
        {
            rankAPI = new RankAPI();
        }
        private bool _loading = true;
        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; DoPropertyChanged("Loading"); }
        }
        private RankRegionModel _current;

        public RankRegionModel Current
        {
            get { return _current; }
            set { _current = value; DoPropertyChanged("Current"); }
        }
        private List<RankRegionModel> _RegionItems;
        public List<RankRegionModel> RegionItems
        {
            get { return _RegionItems; }
            set { _RegionItems = value; DoPropertyChanged("RegionItems"); }
        }
        public void LoadRankRegion(int rid = 0)
        {
            try
            {

                Loading = true;
                //var results = await rankAPI.RankRegion().Request();
                //if (results.status)
                //{
                //var data = await results.GetJson<ApiDataModel<List<RankRegionModel>>>();
                //if (data.success)
                //{
                RegionItems = new List<RankRegionModel>() {
                    new RankRegionModel(0,"全站"),
                    new RankRegionModel(0,"原创", RankRegionType.origin),
                    new RankRegionModel(0,"新人", RankRegionType.rookie),
                    new RankRegionModel(1,"动画"),
                    new RankRegionModel(168,"国创相关"),
                    new RankRegionModel(3,"音乐"),
                    new RankRegionModel(129,"舞蹈"),
                    new RankRegionModel(4,"游戏"),
                    new RankRegionModel(36,"知识"),
                    new RankRegionModel(188,"数码"),
                    new RankRegionModel(160,"生活"),
                    new RankRegionModel(211,"美食"),
                    new RankRegionModel(119,"鬼畜"),
                    new RankRegionModel(155,"时尚"),
                    new RankRegionModel(5,"娱乐"),
                    new RankRegionModel(181,"影视"),

                };
                Current = RegionItems.FirstOrDefault(x => x.rid == rid);
                //    }
                //    else
                //    {
                //        Notify.ShowMessageToast(data.message);
                //    }
                //}
                //else
                //{
                //    Notify.ShowMessageToast(results.message);

                //}
            }
            catch (Exception ex)
            {
                var handel = HandelError<ApiDataModel<List<RankRegionModel>>>(ex);
                Notify.ShowMessageToast(handel.message);
            }
            finally
            {
                Loading = false;
            }
        }
        public async Task LoadRankDetail(RankRegionModel region)
        {
            try
            {
                Loading = true;
                var results = await rankAPI.Rank(region.rid, region.type.ToString()).Request();
                if (results.status)
                {
                    var data = await results.GetJson<ApiDataModel<JObject>>();
                    if (data.success)
                    {
                        region.ToolTip = data.data["note"].ToString();
                        var result = await data.data["list"].ToString().DeserializeJson<List<RankItemModel>>();
                        int i = 1;
                        result = result.ToList();
                        foreach (var item in result)
                        {
                            item.rank = i;
                            i++;
                        }
                        region.Items = result;
                    }
                    else
                    {
                        Notify.ShowMessageToast(data.message);
                    }
                }
                else
                {
                    Notify.ShowMessageToast(results.message);

                }
            }
            catch (Exception ex)
            {
                var handel = HandelError<ApiDataModel<List<RankRegionModel>>>(ex);
                Notify.ShowMessageToast(handel.message);
            }
            finally
            {
                Loading = false;
            }
        }
    }
    public enum RankRegionType
    {
        /// <summary>
        /// 全部
        /// </summary>
        all,
        /// <summary>
        /// 原创
        /// </summary>
        origin,
        /// <summary>
        /// 新人
        /// </summary>
        rookie
    }
    public class RankRegionModel : IModules
    {
        public RankRegionModel(int id, string rname, RankRegionType type = RankRegionType.all)
        {
            this.rid = id;
            this.name = rname;
            this.type = type;
        }
        public string name { get; set; }
        public int rid { get; set; }
        private string _tooltip = "";
        public string ToolTip
        {
            get { return _tooltip; }
            set { _tooltip = value; DoPropertyChanged("ToolTip"); }
        }

        public RankRegionType type { get; set; }

        private List<RankItemModel> _Items;
        public List<RankItemModel> Items
        {
            get { return _Items; }
            set { _Items = value; DoPropertyChanged("Items"); }
        }
    }
    public class RankItemModel
    {
        public int rank { get; set; }
        public string aid { get; set; }
        public int videos { get; set; }
        public int tid { get; set; }
        public string tname { get; set; }
        public int copyright { get; set; }
        public string pic { get; set; }
        public string title { get; set; }
        public int pubdate { get; set; }
        public int ctime { get; set; }
        public string desc { get; set; }
        public int state { get; set; }
        public int duration { get; set; }
        public int mission_id { get; set; }

        public RankItemOwnerModel owner { get; set; }
        public RankItemStatModel stat { get; set; }
        public string dynamic { get; set; }
        public long cid { get; set; }

        public string bvid { get; set; }
        public int score { get; set; }
    }
    public class RankItemStatModel
    {
        public long aid { get; set; }
        public long view { get; set; }
        public long danmaku { get; set; }
        public long reply { get; set; }
        public long favorite { get; set; }
        public long coin { get; set; }
        public long share { get; set; }
    }
    public class RankItemOwnerModel
    {
        public long mid { get; set; }
        public string name { get; set; }
        public string face { get; set; }
    }


}
