   M S B A M L   `   `   `  ����F  BBookManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BookManagerBookManager  5   	   ?$clr-namespace:BookManager.ViewModelsBookManager.ViewModels  5   	       BookManager.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BookManager     5   	   ;
viewModels$clr-namespace:BookManager.ViewModels     5   	     � Title$  Book Manager��5	   	   $	��450��6   $	��900��6+    � DataContext 5      *   $BookManager.ViewModels.MainViewModel  5   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0 0 0 10q�6.   $��0��6@   .+�5      �� $	��200��6   $��5q�6"   ��5      �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      $	��150��6   $��5q�6#   ��5      �� *
Categories+ �� SelectedItem 5      �� *SelectedCategory+�� 5      $��	초기화��6   $��70��6#   $��5q�6.   ��69   �� *ClearFilterCommand+�� 5       $��CSV 내보내기��6   $	��100��6(   $��5q�64   ��6?   �� *ExportCsvCommand+�� 5!      $��	프린트��6   $��80��6#   $��5q�6.   ��69   �� *PrintCommand+5"      �� 5%   
   $ 
Horizontal=�6   $��0 0 0 10q�6.   $��1��6@   .+�5&      �� $	��100��6   $��5q�6"   ��6-   �� *Title+$ PropertyChanged=��� 5'      $	��100��6   $��5q�6"   ��6-   �� *Author+$ PropertyChanged=��� 5(      $	��100��6   $��5q�6"   ��6-   �� *	Publisher+$ PropertyChanged=�(  "System.Windows.Controls.DatePicker  5)      $	��120��6   $��5q�6%      SelectedDate 60   �� *PublishedDate+�� 5*      $	��100��6   $��5q�6"   ��6-   �� *
Category+$ PropertyChanged=��� 5+      $��추가��6   $��50��6"   $��5q�6-   ��68   �� *
AddCommand+�� 5,      $��수정��6   $��50��6"   $��5q�6-   ��68   �� *UpdateCommand+�� 5-      $��삭제��6   $��50��6"   $��5q�6-   ��68   �� *DeleteCommand+�� 5.      $��전체 삭제��6   $��70��6%   $��5q�60   ��6;   �� *ClearAllCommand+5/      &   System.Windows.Controls.DataGrid  52   
   $��2��6      AutoGenerateColumns$ False��54         
IsReadOnly$
 True��6/   ��52       �� *FilteredBooks+ 53      �� *SelectedBook+	 �� Mode$	 TwoWay=�
   Columns
 55      0  *System.Windows.Controls.DataGridTextColumn  56      ,  &System.Windows.Controls.DataGridColumn   Header$ 제목��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Title+  57      $ 저자��6%    61   �� *Author+  58      $ 	출판사��6%    62   �� *	Publisher+  59      $ 	출판일��6%    62   �� *PublishedDate+ �� StringFormat$ d��  5:      $ 카테고리��6%    63   �� *
Category+5<      �� 5?   
   $��3��6   $ ��카테고리별 도서 수��6    $��0,10,0,0q�64   .��5@      �� ��6   �� *CategoryStats+��5A      �� 5B      .R�5C      �� ��6$   �� 5D      5F      5G      �� 5I   
   $��4��6   $*��$최근 추가된 도서 (최신 순)��6    $��0,10,0,0q�6:   .��5J      �� ��6   �� *RecentBooks+��5K      �� 5L      .R�5M      �� ��6$   �� *Title+5N      5P      5Q      5R      5S      