Цель работы: Изучить назначение лексического анализатора. Спроектировать алгоритм и выполнить программную реализацию сканера.

В соответствии с вариантом задания необходимо:

Спроектировать диаграмму состояний сканера (примеры диаграмм представлены в прикрепленных файлах).
Разработать лексический анализатор, позволяющий выделить в тексте лексемы, иные символы считать недопустимыми (выводить ошибку).
Встроить сканер в ранее разработанный интерфейс текстового редактора. Учесть, что текст для разбора может состоять из множества строк.

Вариант - 47. Объявление перечисления в СУБД PostgreSQL
create type request_state as enum ('created', 'approved', 'finshed');

Примеры допустимых строк:
create type bus as enum ('mersedec', 'honda');
create type cifri_ot_1_do_2 as enum ('odin', 'dwa');

Диаграмма состояний сканера:
![image](https://github.com/user-attachments/assets/64f1bc20-6ace-47fe-bd48-b2f10d523b66)

Тест строки: create type bus as enum ('mersedec', 'honda');
Результат анализа:
![image](https://github.com/user-attachments/assets/33343387-4e5c-4563-b50b-4c9165529dc8)

Тест строки: create type cifri_ot_1_do_2 as enum ('odin', 'dwa');
Результат анализа:
![image](https://github.com/user-attachments/assets/083ffb4c-ebca-4902-9b09-add704637791)




