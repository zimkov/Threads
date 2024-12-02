# Threads
 Лабораторные по Параллельному программированию

# Лаборатораня 1

# Лабораторная 2

## Задание

### Последовательный алгоритм "Решето Эратосфена".
Алгоритм заключается в последовательном переборе уже известных простых чисел, начиная с двойки, и проверке разложимости всех чисел диапазона на найденное простое число . На первом шаге выбирается число , проверяется разложимость чисел диапазона на 2-ку. Числа, которые делятся на двойку, помечаются как составные и не участвуют в дальнейшем анализе. Следующим непомеченным (простым) числом будет , и так далее.

![alt text](https://intuit.ru/EDI/25_01_16_3/1453674113-15194/tutorial/1158/objects/6/files/2_1.png)

При этом достаточно проверить разложимость чисел на простые числа в интервале . Например, в интервале от 2 до 20 проверяем все числа на разложимость 2, 3. Составных чисел, которые делятся только на пятерку, в этом диапазоне нет.

### Модифицированный последовательный алгоритм поиска
В последовательном алгоритме "базовые" простые числа определяются поочередно. После тройки следует пятерка, так как четверка исключается при обработке двойки. Последовательность нахождения простых чисел затрудняет распараллеливание алгоритма. В модифицированном алгоритме выделяются два этапа:
1-ый этап: поиск простых чисел в интервале от с помощью классического метода решета Эратосфена (базовые простые числа).
2-ой этап: поиск простых чисел в интервале от , в проверке участвуют базовые простые числа, выявленные на первом этапе.

![alt text](https://intuit.ru/EDI/25_01_16_3/1453674113-15194/tutorial/1158/objects/6/files/2_2.png)

На первом этапе алгоритма выполняется сравнительно небольшой объем работы, поэтому нецелесообразно распараллеливать этот этап. На втором этапе проверяются уже найденные базовые простые числа. Параллельные алгоритмы разрабатываются для второго этапа.

