# Маленькое послание
Добрый день! Хотелось сказать пару слов о том, что было сделано и почему именно так.

Собственно, так как одной из составляющих программы является парсинг html кода, было намечено два варианта, как это делать:

1) Использовать сторонний парсер наподобие HTMLAgilePack 
2) Написать парсер вручную.

Мною был выбран второй вариант т.к из условия непонятно можно ли было использовать сторонние библиотеки или нет. 

Также существует проблема, которую я осознаю, а именно, не ефективное использование многопоточности. 
Суть проблемы заключается в том что потоки попадают в ссылки, которые ведут в никуда или в файлы, из-за чего они завершают работу.
По хорошему эту часть нужно было бы переделать. Сразу эту проблему не распознал, а переделать не хватило времени.

Программа не учитывает ссылки находящиеся в отдельных .js .css файлах

На этом у меня все. Спасибо за уделенное время!
