using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Goat.OpenControls.Base.HandyControl.Data;
using Goat.OpenControls.Base.HandyControl.Interactivity;
using Goat.OpenControls.Base.HandyControl.Tools;
using Goat.OpenControls.Base.HandyControl.Tools.Extension;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    /// <summary>
    ///     页码
    /// </summary>
    [TemplatePart(Name = ElementButtonLeft, Type = typeof(Button))]
    [TemplatePart(Name = ElementButtonRight, Type = typeof(Button))]
    [TemplatePart(Name = ElementButtonFirst, Type = typeof(RadioButton))]
    [TemplatePart(Name = ElementMoreLeft, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementPanelMain, Type = typeof(Panel))]
    [TemplatePart(Name = ElementMoreRight, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementButtonLast, Type = typeof(RadioButton))]
    [TemplatePart(Name = ElementButtonLast, Type = typeof(NumericUpDown))]
    public class Pagination : Control
    {
        #region Constants

        private const string ElementButtonLeft = "PART_ButtonLeft";
        private const string ElementButtonRight = "PART_ButtonRight";
        private const string ElementButtonFirst = "PART_ButtonFirst";
        private const string ElementMoreLeft = "PART_MoreLeft";
        private const string ElementPanelMain = "PART_PanelMain";
        private const string ElementMoreRight = "PART_MoreRight";
        private const string ElementButtonLast = "PART_ButtonLast";
        private const string ElementJump = "PART_Jump";

        #endregion Constants

        #region Data

        private Button _buttonLeft;
        private Button _buttonRight;
        private RadioButton _buttonFirst;
        private RadioButton _moreLeft;
        private Panel _panelMain;
        private RadioButton _moreRight;
        private RadioButton _buttonLast;
        private NumericUpDown _jumpNumericUpDown;

        private bool _appliedTemplate;

        #endregion Data

        #region Public Events

        /// <summary>
        ///     页面更新事件
        /// </summary>
        public static readonly RoutedEvent PageUpdatedEvent =
            EventManager.RegisterRoutedEvent("PageUpdated", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<int>>), typeof(Pagination));

        /// <summary>
        ///     页面更新事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<int>> PageUpdated
        {
            add => AddHandler(PageUpdatedEvent, value);
            remove => RemoveHandler(PageUpdatedEvent, value);
        }

        #endregion Public Events

        public Pagination()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Prev, ButtonPrev_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Next, ButtonNext_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Selected, ToggleButton_OnChecked));
            CommandBindings.Add(new CommandBinding(ControlCommands.Jump, (s, e) => PageIndex = (int)_jumpNumericUpDown.Value));

            OnAutoHidingChanged(AutoHiding);
            //Update();
        }

        #region Public Properties

        #region MaxPageCount

        /// <summary>
        ///     最大页数
        /// </summary>
        public static readonly DependencyProperty MaxPageCountProperty = DependencyProperty.Register(
            "MaxPageCount", typeof(int), typeof(Pagination), new PropertyMetadata(ValueBoxes.Int1Box, OnMaxPageCountChanged, CoerceMaxPageCount), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        private static object CoerceMaxPageCount(DependencyObject d, object basevalue)
        {
            var intValue = (int)basevalue;
            return intValue < 1 ? 1 : intValue;
        }

        private static void OnMaxPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                if (pagination.PageIndex > pagination.MaxPageCount)
                {
                    pagination.PageIndex = pagination.MaxPageCount;
                }

                pagination.CoerceValue(PageIndexProperty);
                pagination.OnAutoHidingChanged(pagination.AutoHiding);
                //pagination.Update();
                pagination.Initialize();
            }
        }

        /// <summary>
        ///     最大页数
        /// </summary>
        public int MaxPageCount
        {
            get => (int)GetValue(MaxPageCountProperty);
            set => SetValue(MaxPageCountProperty, value);
        }

        #endregion MaxPageCount

        #region DataCountPerPage

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public static readonly DependencyProperty DataCountPerPageProperty = DependencyProperty.Register(
            "DataCountPerPage", typeof(int), typeof(Pagination), new PropertyMetadata(20, OnDataCountPerPageChanged),
            ValidateHelper.IsInRangeOfPosInt);

        private static void OnDataCountPerPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                //pagination.Update();
                pagination.Initialize();
            }
        }

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public int DataCountPerPage
        {
            get => (int)GetValue(DataCountPerPageProperty);
            set => SetValue(DataCountPerPageProperty, value);
        }

        #endregion

        #region PageIndex

        /// <summary>
        ///     当前页
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(Pagination), new PropertyMetadata(ValueBoxes.Int1Box, OnPageIndexChanged, CoercePageIndex), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        private static object CoercePageIndex(DependencyObject d, object basevalue)
        {
            if (d is not Pagination pagination) return 1;

            var intValue = (int)basevalue;
            return intValue < 1
                ? 1
                : intValue > pagination.MaxPageCount
                    ? pagination.MaxPageCount
                    : intValue;
        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination && e.NewValue is int value)
            {
                pagination.Update();
                pagination.RaiseEvent(new FunctionEventArgs<int>(PageUpdatedEvent, pagination)
                {
                    Info = value
                });
            }
        }

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        #endregion PageIndex

        #region MaxPageInterval

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>       
        public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
            "MaxPageInterval", typeof(int), typeof(Pagination), new PropertyMetadata(3, OnMaxPageIntervalChanged), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        private static void OnMaxPageIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                //pagination.Update();
                pagination.Initialize();
            }
        }

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>   
        public int MaxPageInterval
        {
            get => (int)GetValue(MaxPageIntervalProperty);
            set => SetValue(MaxPageIntervalProperty, value);
        }

        #endregion MaxPageInterval

        #region IsJumpEnabled

        public static readonly DependencyProperty IsJumpEnabledProperty = DependencyProperty.Register(
            "IsJumpEnabled", typeof(bool), typeof(Pagination), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsJumpEnabled
        {
            get => (bool)GetValue(IsJumpEnabledProperty);
            set => SetValue(IsJumpEnabledProperty, ValueBoxes.BooleanBox(value));
        }

        #endregion

        #region AutoHiding

        public static readonly DependencyProperty AutoHidingProperty = DependencyProperty.Register(
            "AutoHiding", typeof(bool), typeof(Pagination), new PropertyMetadata(ValueBoxes.TrueBox, OnAutoHidingChanged));

        private static void OnAutoHidingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                pagination.OnAutoHidingChanged((bool)e.NewValue);
            }
        }

        private void OnAutoHidingChanged(bool newValue) => this.Show(!newValue || MaxPageCount > 1);

        public bool AutoHiding
        {
            get => (bool)GetValue(AutoHidingProperty);
            set => SetValue(AutoHidingProperty, ValueBoxes.BooleanBox(value));
        }

        #endregion

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            _appliedTemplate = false;
            base.OnApplyTemplate();

            _buttonLeft = GetTemplateChild(ElementButtonLeft) as Button;
            _buttonRight = GetTemplateChild(ElementButtonRight) as Button;
            _buttonFirst = GetTemplateChild(ElementButtonFirst) as RadioButton;
            _moreLeft = GetTemplateChild(ElementMoreLeft) as RadioButton;
            _panelMain = GetTemplateChild(ElementPanelMain) as Panel;
            _moreRight = GetTemplateChild(ElementMoreRight) as RadioButton;
            _buttonLast = GetTemplateChild(ElementButtonLast) as RadioButton;
            _jumpNumericUpDown = GetTemplateChild(ElementJump) as NumericUpDown;

            if (!CheckNull())
            {
                return;
            }

            _appliedTemplate = true;
            //Update();

            Initialize();
        }

        #endregion Public Methods

        #region Private Methods

        //private void CheckNull()
        //{
        //    if (_buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
        //        _moreLeft == null || _panelMain == null || _moreRight == null ||
        //        _buttonLast == null) throw new Exception();
        //}
        private bool CheckNull()
        {
            if (_buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
                _moreLeft == null || _panelMain == null || _moreRight == null ||
                _buttonLast == null) return false;

            return true;
        }


        private int _pageInterval = 3;
        private string _mark = "...";

        private void Initialize()
        {
            if (!_appliedTemplate) return;

            PageIndex = 1;

            _buttonLeft.IsEnabled = PageIndex > 1;
            _buttonRight.IsEnabled = PageIndex < MaxPageCount;
            if (MaxPageInterval > 3)
            {
                _pageInterval = MaxPageInterval;
            }

            if (MaxPageCount <= 4 + _pageInterval)
            {
                _buttonFirst.Show(true);
                _buttonLast.Show(false);
                _moreLeft.Show(false);
                _moreRight.Show(false);

                _panelMain.Children.Clear();
                for (int i = 1; i < MaxPageCount; i++)
                {
                    int number = i + 1;
                    var button = CreateButton(number);
                    if (number == 1)
                    {
                        button.IsChecked = true;
                    }
                    _panelMain.Children.Add(button);
                }

            }
            else
            {
                _buttonFirst.Show();
                _buttonLast.Show();
                _buttonLast.Content = MaxPageCount.ToString();

                _moreLeft.Show(true);
                _moreRight.Show(true);

                ShowHead();
            }
        }

        private void ShowHead()
        {
            int count = MaxPageInterval + 1;
            int number = 1 + 1;
            _moreLeft.Content = number.ToString();
            _panelMain.Children.Clear();
            for (int i = 1; i < count; i++)  //+1是因为多加了一个_moreLeft
            {
                _panelMain.Children.Add(CreateButton(i + number));
            }
            _moreRight.Content = _mark;
        }

        private void ShowEnd()
        {
            int count = MaxPageInterval + 1;
            int number = MaxPageCount - 1;
            _moreLeft.Content = _mark;
            _panelMain.Children.Clear();
            for (int i = 1; i < count; i++)
            {
                _panelMain.Children.Insert(0, CreateButton(number - i));
            }
            _moreRight.Content = number.ToString();

        }


        private void ShowJump()
        {
            int range = MaxPageInterval + 1;
            if (1 < PageIndex && PageIndex < 1 + range)
            {
                ShowHead();
            }
            else if (MaxPageCount - range < PageIndex && PageIndex < MaxPageCount)
            {
                ShowEnd();
            }
            else
            {
                int offset = (int)Math.Floor((double)_pageInterval / 2);
                int number = PageIndex - offset;
                _panelMain.Children.Clear();
                for (int i = 0; i < _pageInterval; i++)
                {
                    _panelMain.Children.Add(CreateButton(number + i));
                }
                _moreLeft.Content = _mark;
                _moreRight.Content = _mark;
            }
        }

        private void Update()
        {
#if true
            if (!_appliedTemplate) return;
            _buttonLeft.IsEnabled = PageIndex > 1;
            _buttonRight.IsEnabled = PageIndex < MaxPageCount;

            _jumpNumericUpDown.Value = PageIndex;
            if (MaxPageCount > 4 + _pageInterval)
            {
                if (PageIndex <= 1)
                {
                    _buttonFirst.IsChecked = true;

                    _buttonFirst.Show();
                    _buttonLast.Show();
                    _buttonLast.Content = MaxPageCount.ToString();
                    _moreLeft.Show(true);
                    _moreRight.Show(true);

                    ShowHead();
                    return;
                }
                else if (PageIndex >= MaxPageCount)
                {
                    _buttonLast.IsChecked = true;
                    ShowEnd();
                    return;
                }

                ShowJump();
            }
            var selectButton = GetRadioButton(PageIndex);
            if (selectButton != null)
            {
                selectButton.IsChecked = true;
            }
#endif

        }

        private bool CheckMoreButton(RadioButton radioButton)
        {
            if (radioButton.Content.ToString() == "...")
            {
                return true;
            }
            return false;
        }

        private int GetNumber(UIElement element)
        {
            int number = -1;
            if (element is RadioButton radioButton)
            {

                if (int.TryParse(radioButton.Content.ToString(), out number))
                {
                    return number;
                }
            }
            return number;
        }

        private RadioButton GetRadioButton(int number)
        {
            if (GetNumber(_moreLeft) == number)
                return _moreLeft;
            else if (GetNumber(_moreRight) == number)
                return _moreRight;

            for (int i = 0; i < _panelMain.Children.Count; i++)
            {
                var child = _panelMain.Children[i];
                if (child is RadioButton radioButton)
                {
                    if (GetNumber(radioButton) == number)
                        return radioButton;
                }
            }
            return null;
        }

#if false
        /// <summary>
        ///     更新
        /// </summary>
        private void Update()
        {
            if (!_appliedTemplate) return;
            _buttonLeft.IsEnabled = PageIndex > 1;
            _buttonRight.IsEnabled = PageIndex < MaxPageCount;
            if (MaxPageInterval == 0)
            {
                _buttonFirst.Collapse();
                _buttonLast.Collapse();
                _moreLeft.Collapse();
                _moreRight.Collapse();
                _panelMain.Children.Clear();
                var selectButton = CreateButton(PageIndex);
                _panelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
                return;
            }
            _buttonFirst.Show();
            _buttonLast.Show();
            _moreLeft.Show();
            _moreRight.Show();

            //更新最后一页
            if (MaxPageCount == 1)
            {
                _buttonLast.Collapse();
            }
            else
            {
                _buttonLast.Show();
                _buttonLast.Content = MaxPageCount.ToString();
            }

            //更新省略号
            var right = MaxPageCount - PageIndex;
            var left = PageIndex - 1;
            _moreRight.Show(right > MaxPageInterval);
            _moreLeft.Show(left > MaxPageInterval);

            //更新中间部分
            _panelMain.Children.Clear();
            if (PageIndex > 1 && PageIndex < MaxPageCount)
            {
                var selectButton = CreateButton(PageIndex);
                _panelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
            }
            else if (PageIndex == 1)
            {
                _buttonFirst.IsChecked = true;
            }
            else
            {
                _buttonLast.IsChecked = true;
            }

            var sub = PageIndex;
            for (var i = 0; i < MaxPageInterval - 1; i++)
            {
                if (--sub > 1)
                {
                    _panelMain.Children.Insert(0, CreateButton(sub));
                }
                else
                {
                    break;
                }
            }
            var add = PageIndex;
            for (var i = 0; i < MaxPageInterval - 1; i++)
            {
                if (++add < MaxPageCount)
                {
                    _panelMain.Children.Add(CreateButton(add));
                }
                else
                {
                    break;
                }
            }
        }
#endif

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;

        private RadioButton CreateButton(int page)
        {
            return new()
            {
                Style = ResourceHelper.GetResourceInternal<Style>(ResourceToken.PaginationButtonStyle),
                Content = page.ToString()
            };
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not RadioButton button) return;
            if (button.IsChecked == false) return;

            int index = PageIndex;
            bool ret = int.TryParse(button.Content.ToString(), out index);
            string name = button.Name;
            if (ret)
            {
                PageIndex = index;
                return;
            }
            int interval = MaxPageInterval - 1;
            if (name == ElementMoreRight)
            {
                index = PageIndex + interval;
                if (index >= MaxPageCount)
                {
                    PageIndex = MaxPageCount;
                }
                else
                {
                    PageIndex = index;
                }
            }
            else if (name == ElementMoreLeft)
            {
                index = PageIndex - interval;
                if (index <= 1)
                {
                    PageIndex = 1;
                }
                else
                {
                    PageIndex = index;
                }
            }


        }

        #endregion Private Methods       
    }
}
