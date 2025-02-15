====================================
Программа для формирования скрипта
====================================

ЛЕВАЯ ПАНЕЛЬ
------------------------------------

┌───────┐
├───┬───┤
│   │   │
│   │   │
└───┴───┘

В верхней части расположены кнопки:

1. Добавление новой таблицы с колонками
2. Добавление в активную таблицу новых колонок
3. База данных (не работает)
4. Добавление кавычек в список (очень удобная штука, пользуюсь постоянно)
5. Добавление записям колонок кавычек
6. Добавление колонки в сравнение

При добавлении колонки, если задать название через точку, например:
rf_MKBID.DS
то программа будет считать, что добавлена ссылочная таблица, и в основную колонку запишется rf_MKBID и заполнится нижняя часть 
* Ключ (уберется rf_ из rf_MKBID)
* Таблица (уберется rf из rf_MKBID + добавится префикс oms)
* Колонка (заполнится DS)
P.s. Псевдоним заполнять самостоятельно 

Так же реализована логика, что если уже когда-то раньше присоединяли колонку с таким же названием (rf_MKBID), то будет подставляться уже будет тот вариант, который сохранен ранее в бд, например, hlt_MKB

ОБЛАСТИ ПАНЕЛИ
------------------------------------

- Левая часть — история созданных таблиц.
- Правая часть — отображаются колонки, относящиеся к таблице.

* С помощью мыши можно перетаскивать таблицы или колонки вверх-вниз в рамках панели.

* Правой кнопкой мыши можно удалить ненужные записи.
  - Для колонок есть дополнительная возможность добавить их в сравнение.

====================================
ОСНОВНАЯ ФОРМА
====================================

Форма содержит:
- Два больших текстовых поля
- Пять строчных текстовых полей

Строчные поля
------------------------------------

Верхняя часть:
1. Таблица — наименование актуальной таблицы
2. Колонка — наименование актуальной колонки

Строчные поля для присоединения таблицы:
3. Ключ связанной таблицы — поле, с которым сравнивается "Колонка", по которому присоединяется таблица
4. Связанная таблица — присоединяемая таблица
5. Колонка — вводное поле из связанной таблицы

Большие текстовые поля
------------------------------------

1. Записи — в него записывается список значений актуальной колонки.
2. Скрипт — сформированный итоговый скрипт.

====================================
КНОПКИ НАД ТЕКСТОВЫМ ПОЛЕМ "СКРИПТ"
====================================

1. Выгрузка в SQL-файл
   P.S. В поле выбора файла необходимо указать папку (Emias), например:
   	D:\Projects\whc-scripts\src\Production\Emias
   При выгрузке скрипта будет автоматически добавлен шаблон.

2. Добавление шаблона в "Скрипт"
