   M S B A M L   `   `   `  ����  Practice#    Practice.Pages.CategoriesPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  CategoriesPage��5       N� 
Background
 �`���.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      �� $��Поиск:��6   $��Center=�6%   �� 5      -   5         ��Name$ SearchTextBox��5      $	��300��6-   $	��5,0q�69   6F   5      �� 5   
   -   5         ��Name$ CategoriesListView��5      $��1��6/   $��0,0,0,10q�6<    �� View 5      �� 5      .P�5      �� $��ID��6%    �� Width$ 50��6U    �� DisplayMemberBinding 61   �� *id+6U   �� 5      $
��Name��6%   $	 200��6Y    63   �� *name+6Y   5      5      �� 5   
   $ 
Horizontal=�6   $��2��6.   $��Right=�6;   .+�5      �� -   5        $��Добавить��5      $	��100��6(   $��5q�64   6?   �� 5       -   5        $ ��Редактировать��5       $	��100��6-   $��5q�69   6D   �� 5!      -   5        $��Удалить��5!      $	��100��6'   $��5q�63   6>   5"      5#      5$      