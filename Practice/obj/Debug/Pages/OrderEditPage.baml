   M S B A M L   `   `   `  ����  Practice"    Practice.Pages.OrderEditPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  OrderEditPage��5       N� 
Background
 �`���.��5      � $��10q�6   .+�5   
   �� .+�5
      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��Product ID:��6   $��Center=�6/   $��0,0,5,0q�6J   �� 5      -   5         ��Name$ ProductIdTextBox��5      $	��200��64   6@   5      �� 5      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��Product Name:��6   $��Center=�61   $��0,0,5,0q�6L   �� 5      -   5        $ ProductNameTextBox��5      $	��200��66    �� 
IsReadOnly . 6B   5      �� 5      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��User ID:��6   $��Center=�6,   $��0,0,5,0q�6G   �� 5      -   5        $ UserIdTextBox��5      $	��200��61   5      �� 5      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��Price:��6   $��Center=�6*   $��0,0,5,0q�6E   �� 5      -   5        $ PriceTextBox��5      $	��200��60   6<   5      �� 5"      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5#      �� $��Count:��6   $��Center=�6*   $��0,0,5,0q�6E   �� 5$      -   5        $ CountTextBox��5$      $	��200��60   6<   5%      �� 5(      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5)      �� $
��Sum:��6   $��Center=�6(   $��0,0,5,0q�6C   �� 5*      -   5        $ 
SumTextBox��5*      $	��200��6.    . 6:   5+      �� 5.      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5/      �� $��Date Order:��6   $��Center=�6/   $��0,0,5,0q�6J   (  "System.Windows.Controls.DatePicker  50      -   5          Name$ DateOrderPicker��50         SelectedDateFormat$ Short=�66   $	��200��6Q   51      52      �� 55   
   $ 
Horizontal=�6   $��Bottom=�6.   $��Right=�6I   $��0,0,0,10q�6e   .+�56      �� -   5        $��Сохранить��56      $��0,0,5,0q�6B   �� 57      -	   5        $��Отмена��57      6&   58      59      5:      