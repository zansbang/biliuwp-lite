﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using AutoMapper;
using BiliLite.Extensions;
using BiliLite.Models.Common.Video;
using BiliLite.ViewModels.Video;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace BiliLite.Controls
{
    public sealed partial class VideoListView : UserControl
    {
        private readonly VideoListViewModel m_viewModel;
        private readonly IMapper m_mapper;

        public VideoListView(VideoListViewModel viewModel, IMapper mapper)
        {
            m_viewModel = viewModel;
            m_mapper = mapper;
            this.InitializeComponent();
        }

        public event EventHandler<VideoListItem> OnSelectionChanged;

        public void LoadData(List<VideoListSection> sections)
        {
            if(m_viewModel.Sections == null)
                m_viewModel.Sections = m_mapper.Map<ObservableCollection<VideoListSectionViewModel>>(sections);
            else
            {
                var mapSections = m_mapper.Map<List<VideoListSectionViewModel>>(sections);
                m_viewModel.Sections.AddRange(mapSections);
            }
        }

        public void Next(string id)
        {
            var next = false;
            foreach (var section in m_viewModel.Sections)
            {
                foreach (var item in section.Items)
                {
                    if (next)
                    {
                        section.SelectedItem = item;
                        return;
                    }
                    if (item.Id == id)
                    {
                        next = true;
                    }
                }
            }
            Notify.ShowMessageToast("播放完毕");
        }

        public bool IsLast(string id)
        {
            return m_viewModel.Sections.LastOrDefault()?.Items.LastOrDefault()?.Id == id;
        }

        public VideoListItem CurrentItem()
        {
            return (m_viewModel.Sections.Where(section => section.SelectedItem != null)
                .Select(section => section.SelectedItem)).FirstOrDefault();
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListView { SelectedItem: VideoListItem item })) return;
            foreach (var section in m_viewModel.Sections.Where(
                         x => x.SelectedItem != item && x.SelectedItem != null))
            {
                section.SelectedItem = null;
            }
            ScrollToItem(item);

            OnSelectionChanged?.Invoke(this, item);
        }

        private void ScrollToItem(VideoListItem item)
        {
            //TODO: 实现滚动到指定item
            //SectionListView.ScrollIntoView();
        }
    }
}