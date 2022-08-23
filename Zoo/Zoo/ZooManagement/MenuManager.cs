using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.ZooManagement
{
    public class MenuManager
    {
        private readonly CageHandler _cageHandler;
        private readonly AnimalHandler _animalHandler;
        public MenuManager(CageHandler cageHandler, AnimalHandler animalHandler)
        {
            _cageHandler = cageHandler;
            _animalHandler = animalHandler;
        }
        public Cage HandleCreateCage()
        {
            Console.WriteLine("Nhập thông tin để tạo Lồng");

            Console.WriteLine("Code: ");
            string code = Console.ReadLine();

            Console.WriteLine("Name: ");
            string name = Console.ReadLine();

            Cage cage = new Cage(code, name);

            return cage;
        }
        public void HandleSelectCage(List<Cage> cages)
        {
            string cageCode = InputCageCode();

            Cage cage = _cageHandler.FindCage(cages, cageCode);


            Console.WriteLine("\nThông tin Lồng:");
            cage.ShowInfo();

            //_animalHandler.ShowInfoAnimals(cage.animals);
            Console.WriteLine("\n=========DANH SÁCH ĐỘNG VẬT==========");
            foreach (var animal in cage.animals)
            {
                animal.ShowInFo();
            }

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
                            _cageHandler.AddAnimalToCage(cage, animalChose);
                            _animalHandler.ShowInfoAnimals(cage.animals);
                        } while (animalChose != 0);
                        break;
                    case 2:
                        ///
                        break;
                    case 3:
                        TimeForEat(cage);
                        break;
                    case 4:
                        //
                        break;
                    default:
                        return;
                }
            } while (chose != 0);
        }

        private string InputCageCode()
        {
            Console.WriteLine($"Nhập mã Lồng: ");
            string input = Console.ReadLine();

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
            cage.TimeForEat(1);
        }

        private int valueChoseInMenu()
        {
            string input = Console.ReadLine();
            int value = Int32.Parse(input);

            return value;
        }

    }
}
