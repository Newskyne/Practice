   M S B A M L   `   `   `  ����  Practice$    Practice.Pages.ProductEditPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$J  DДобавление/Редактирование продукции��5      .��5      � $��10q�6   
���`��� � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   �� .+�5      �� $)��#Название продукции��6   $
��Bold"�62   �� 5      -   5         ��Name$ ProductNameTextBox��5      $	��0,5q�62   �� 5      $��Категория��6   $
��Bold"�6)   �� 5      -   5         ��Name$ CategoryComboBox��5       �� DisplayMemberPath$
 name��61    �� SelectedValuePath$ id��5      $	��0,5q�6.   �� 5      -   5         ��Name$ InfoTextBlock��5      
���  ��$
��0,10q�6@   5      �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        $��Сохранить��5      $��5q�6)   64   �� 5      -   5        $��Отмена��5      $��5q�6&   61   5      5      5      