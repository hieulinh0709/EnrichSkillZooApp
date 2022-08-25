using System;
using System.Collections.Generic;

namespace Zoo.ZooManagement
{
    public enum Food
    {
        Seed = 1,
        Meat = 2
    }
    public class MenuManager
    {
        private readonly ZooManager _zooManager;
        public MenuManager() { }
        public MenuManager(ZooManager zooManager)
        {
            _zooManager = zooManager;
        }

        /// <summary>
        /// My comment
        /// </summary>
        /// <param name="cages"></param>
        public void HandleSelectCage(List<Cage> cages)
        {
            string cageCode = InputCageCode();
            Cage cage = _zooManager.FindCage(cages, Guid.Parse(cageCode));

            if (cage == null)
                throw new ArgumentNullException($"Không tìm thấy Lồng với Mã: {cageCode}");

            Console.WriteLine("\nThông tin Lồng:");
            cage.ShowInfo();
            cage.ShowInfoAnimals(cage.animals);

            int chose = 999;
            do
            {
                MenuAnimal();

                chose = valueChoseInMenu();
                switch (chose)
                {
                    case 1:
                        // Thêm động vật
                        int animalChose = 999;
                        do
                        {
                            ListDefaultAnimal();
                            animalChose = valueChoseInMenu();
                            _zooManager.AddAnimalToCage(cage, animalChose);
                            cage.ShowInfoAnimals(cage.animals);
                        } while (animalChose != 0);
                        break;
                    case 2:
                        string animalCode = InputAnimalCode();
                        
                        _zooManager.RemoveAnimalFromCage(cage, Guid.Parse(animalCode));
                        cage.ShowInfoAnimals(cage.animals);
                        break;
                    case 3:
                        TimeForEat(cage);
                        break;
                    default:
                        return;
                }
            } while (chose != 0);
        }

        public void HandleRemoveCage(List<Cage> cages)
        {
            string cageCode = InputCageCode();
            _zooManager.RemoveCage(cages, Guid.Parse(cageCode));
        }
        private string InputAnimalCode()
        {
            Console.WriteLine($"Nhập mã động: ");
            bool isValid = true;
            string input = string.Empty;
            do
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    isValid = false;
            } while (!isValid);

            return input;
        }
        private string InputCageCode()
        {
            Console.WriteLine($"Nhập mã Lồng: ");
            bool isValid = true;
            string input = string.Empty;
            do
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    isValid = false;
            } while (!isValid);

            return input;
        }

        private void MenuAnimal()
        {
            Console.WriteLine("\n=====================================");
            Console.WriteLine("1. Thêm Động Vật");
            Console.WriteLine("2. Xóa động vật");
            Console.WriteLine("3. Cho ăn");
            Console.WriteLine("0. Quay lại.");
            Console.WriteLine("Nhập lựa chọn của bạn: ");
        }

        public void MenuCage()
        {
            Console.WriteLine("\n=====================================");
            Console.WriteLine("1. Tạo lồng");
            Console.WriteLine("2. Chọn lồng");
            Console.WriteLine("3. Xóa lồng");
            Console.WriteLine("4. Thoát");
            Console.WriteLine("Nhập lựa chọn của bạn: ");

        }

        /// <summary>
        /// Here we can loop in the list of the animal to show
        /// </summary>
        public void ListDefaultAnimal()
        {
            Console.WriteLine("\n=====================================");
            Console.WriteLine("1. Wolf");
            Console.WriteLine("2. Pig");
            Console.WriteLine("3. Monkey");
            Console.WriteLine("4. Parrot");
            Console.WriteLine("0. Exit");
            Console.WriteLine("Nhập lựa chọn của bạn: ");

        }

        private void TimeForEat(Cage cage)
        {
            Console.WriteLine("\nChọn loại thức ăn");
            Console.WriteLine("1. Seed");
            Console.WriteLine("2. Meat");
            int chose = valueChoseInMenu();

            foreach (var animal in cage.animals)
            {
                animal.Sounding = false;
            }
            cage.TimeForEat((Food)chose);
        }

        private int valueChoseInMenu()
        {
            bool isValid = true;
            int value;

            do
            {
                string input = Console.ReadLine();
                bool isOK = Int32.TryParse(input, out value);
            } while (!isValid);

            return value;
        }

    }
}
