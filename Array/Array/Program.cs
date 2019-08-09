using System;

namespace Array
{
    internal class Matrix
    {
        private int[,] array = new int[0, 0];
        public Matrix(int size)
        {
            RowCount = ColumnCount = size;
            array = new int[size, size];
        }
        public Matrix(int rows, int columns)
        {
            RowCount = rows;
            ColumnCount = columns;
            array = new int[rows, columns];
        }
        public bool IsSquare => RowCount == ColumnCount;
        public int RowCount { get; }
        public int ColumnCount { get; }

        public void GenerateArray()
        {
            Random random = new Random();
            for (int i = 0; i < RowCount; i++)
                for (int j = 0; j < ColumnCount; j++)
                    array[i, j] = random.Next(0, 4);
        }
        public void DeleteDuplicates()
        {
            while (DeleteDuplicatesInRows()) ;
            while (DeleteDuplicatesInColumns()) ;
        }
        private bool DeleteDuplicatesInRows()
        {
            bool IsDuplicatesExist = false;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 1; j < ColumnCount-1; j++)
                {
                    //если элементы слева и справа равны текущему
                    if (array[i, j - 1] == array[i, j] && array[i, j] == array[i, j + 1])//три в строке одинаковых
                    {
                        IsDuplicatesExist = true;
                        int currentEndLineColumnIndex = j + 1;
                        int startLineIndex = j - 1;
                        while (currentEndLineColumnIndex < ColumnCount-1 && array[i, currentEndLineColumnIndex] == array[i, currentEndLineColumnIndex + 1])
                            currentEndLineColumnIndex++;
                        int otherDigitColumnIndex = currentEndLineColumnIndex + 1;
                        MoveColumns(array, i, startLineIndex, otherDigitColumnIndex);
                    }
                }
            }
            return IsDuplicatesExist;
        }
        /// <summary>
        /// Сдвигает элементы в строке
        /// </summary>
        /// <param name="array">Массив элементов</param>
        /// <param name="rowIndex">Индекс строки</param>
        /// <param name="startLineIndex">Индекс столбца, с которого началась линия повторяющихся элементов</param>
        /// <param name="otherDigitColumnIndex">Индекс столбца, на котором линия закончилась</param>
        private void MoveColumns(int[,] array, int rowIndex, int startLineIndex, int otherDigitColumnIndex)
        {
            Random random = new Random();
            int lineSize = otherDigitColumnIndex - startLineIndex;//длина линии с повторяющимися элементами
            bool fromRight = false, fromLeft = false;//с какой стороны будут подтягиваться элементы?

            if (lineSize > (ColumnCount - otherDigitColumnIndex) || (otherDigitColumnIndex > ColumnCount-1))//если линия находится так, что справа элементов меньше, чем элементов в самой линии
                fromLeft = true;//будем двигать слева направо
            else
                fromRight = true;//иначе будем двигать справа налево
            try
            {
                //================================================================================================================
                /*Сдвигаем элементы на места элементов линии*/
                for (int j = startLineIndex; j < otherDigitColumnIndex; j++)
                {
                    if (fromRight && (j + lineSize) < ColumnCount)
                        array[rowIndex, j] = array[rowIndex, j + lineSize];//берем правые элементы и вставляем на место повторяющихся
                    else if (fromLeft && (j - lineSize) >= 0)
                        array[rowIndex, j] = array[rowIndex, j - lineSize];//берем левые элементы и вставляем на место повторяющихся
                    else
                        array[rowIndex, j] = random.Next(0, 4);//заполняем рандомно
                }
                //================================================================================================================
                /*Сдвигаем циклично кол-во элементов за линией*/
                if (fromRight && (otherDigitColumnIndex + lineSize) <= array.GetUpperBound(1))//если двигали справа налево и кол-во элементов для сдвига не превышает кол-во оставшихся справа элементов
                    for (int j = otherDigitColumnIndex; j < otherDigitColumnIndex + lineSize; j++)//с индекса, с которого линия прекратилась до индекса, с которого линия прекратилась + размер линии
                        if (j + lineSize < ColumnCount)//если элементов для сдвига меньше, чем повторяющихся, то сдвигаем сколько есть
                            array[rowIndex, j] = array[rowIndex, j + lineSize];

                if (fromLeft && (startLineIndex - lineSize) >= 0)//если двигали слева направо и кол-во элементов для сдвига не превышает кол-во оставшихся слева элементов
                    for (int j = startLineIndex - lineSize; j < startLineIndex; j++)//с индекса, с которого линия прекратилась до индекса, с которого линия прекратилась + размер линии
                        if (j - lineSize >= 0)//если элементов для сдвига меньше, чем повторяющихся, то сдвигаем сколько есть
                            array[rowIndex, j] = array[rowIndex, j - lineSize];
                //================================================================================================================
                /*После сдвига элементов заполняем рандомно пустоту*/
                if (fromRight)//если двигали справа налево
                    for (int j = ColumnCount - lineSize ; j < ColumnCount; j++)//с (конец строки — длина линии) по конец строки
                        array[rowIndex, j] = random.Next(0, 4);//заполняем рандомно
                else if (fromLeft)//если двигали слева направо
                    for (int j = 0; j < lineSize; j++)//с начала строки по (начало строки + длина линии)
                        array[rowIndex, j] = random.Next(0, 4);//заполняем рандомно

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine($"Exception in row #{rowIndex}\n");
            }
        }
        private bool DeleteDuplicatesInColumns()
        {
            bool IsDuplicatesExist = false;
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 1; i < RowCount-1; i++)
                {
                    //если элементы сверху и снизу равны текущему
                    if (array[i - 1, j] == array[i, j] && array[i, j] == array[i + 1, j])//три в столбце одинаковых
                    {
                        IsDuplicatesExist = true;
                        int currentEndColumnRowIndex = i + 1;
                        int startLineIndex = i - 1;
                        while (currentEndColumnRowIndex < RowCount-1 && array[currentEndColumnRowIndex, j] == array[currentEndColumnRowIndex + 1, j])
                            currentEndColumnRowIndex++;
                        int otherDigitRowIndex = currentEndColumnRowIndex + 1;
                        MoveRows(array, j, startLineIndex, otherDigitRowIndex);
                    }
                }
            }
            return IsDuplicatesExist;
        }
        /// <summary>
        /// Сдвигает элементы в столбце
        /// </summary>
        /// <param name="array">Массив элементов</param>
        /// <param name="columnIndex">Индекс столбца</param>
        /// <param name="startLineIndex">Индекс строки, с которой началась линия повторяющихся элементов</param>
        /// <param name="otherDigitRowIndex">Индекс строки, на которой линия закончилась</param>
        private void MoveRows(int[,] array, int columnIndex, int startLineIndex, int otherDigitRowIndex)
        {
            Random random = new Random();
            int lineSize = otherDigitRowIndex - startLineIndex;//длина линии с повторяющимися элементами
            bool fromBottom = false, fromTop = false;//с какой стороны будут подтягиваться элементы?

            if (lineSize > (RowCount - otherDigitRowIndex) || (otherDigitRowIndex > RowCount-1))//если линия находится так, что снизу элементов меньше, чем элементов в самой линии
                fromTop = true;//будем двигать сверху вниз
            else
                fromBottom = true;//иначе будем двигать снизу вверх
            try
            {
                //================================================================================================================
                /*Сдвигаем элементы на места элементов линии*/
                for (int i = startLineIndex; i < otherDigitRowIndex; i++)
                {

                    if (fromBottom && (i + lineSize) < RowCount)
                        array[i, columnIndex] = array[i + lineSize, columnIndex];//берем нижние элементы и вставляем на место повторяющихся
                    else if (fromTop && (i - lineSize) >= 0)
                        array[i, columnIndex] = array[i - lineSize, columnIndex];//берем верхние элементы и вставляем на место повторяющихся
                    else
                        array[i, columnIndex] = random.Next(0, 4);//заполняем рандомно
                }
                //================================================================================================================
                /*Сдвигаем циклично кол-во элементов за линией*/
                if (fromBottom && (otherDigitRowIndex + lineSize) < RowCount)//если двигали снизу вверх и кол-во элементов для сдвига не превышает кол-во оставшихся снизу элементов
                    for (int i = otherDigitRowIndex; i < otherDigitRowIndex + lineSize; i++)//с индекса, с которого линия прекратилась до индекса, с которого линия прекратилась + размер линии
                        if (i + lineSize < RowCount)//если элементов для сдвига меньше, чем повторяющихся, то сдвигаем сколько есть
                            array[i, columnIndex] = array[i + lineSize, columnIndex];

                if (fromTop && (startLineIndex - lineSize) >= 0)//если двигали сверху вниз и кол-во элементов для сдвига не превышает кол-во оставшихся сверху элементов
                    for (int i = startLineIndex - lineSize; i < startLineIndex; i++)//с индекса, с которого линия прекратилась до индекса, с которого линия прекратилась + размер линии
                        if (i - lineSize >= 0)//если элементов для сдвига меньше, чем повторяющихся, то сдвигаем сколько есть
                            array[i, columnIndex] = array[i - lineSize, columnIndex];
                //================================================================================================================
                /*После сдвига элементов заполняем рандомно пустоту*/
                if (fromBottom)//если двигали снизу вверх
                    for (int i = RowCount - lineSize ; i < RowCount; i++)//с (конец столбца — длина линии) по конец столбца
                        array[i, columnIndex] = random.Next(0, 4);//заполняем рандомно
                else if (fromTop)//если двигали сверху вниз
                    for (int i = 0; i < lineSize; i++)//с начала столбца по (начало столбца + длина линии)
                        array[i, columnIndex] = random.Next(0, 4);//заполняем рандомно
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine($"Exception in column #{columnIndex}");
            }
        }
        public override string ToString()
        {
            string outputString = "";
            for (int i = 0; i < RowCount; i++)
            {
                outputString += $"#{i + 1}\t";
                for (int j = 0; j < ColumnCount; j++)
                    outputString += $"{array[i, j]} ";
                outputString += "\n";
            }
            return outputString;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(9);

            matrix.GenerateArray();
            Console.WriteLine(matrix);

            matrix.DeleteDuplicates();
            Console.WriteLine(matrix);

            Console.ReadLine();
        }

    }
}
