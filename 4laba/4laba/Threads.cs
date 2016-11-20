using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _4laba
{
    public class Threads
    {
        private ChangeUI _ui;

        public Threads(ChangeUI ui)
        {
            _ui = ui;
        }

        private List<Thread> _threads;

        delegate void set_delegate(object Name);
        delegate void save_delegate(object Name);
        delegate void print_delegate(object Name);
        Thread thread;
        public void Run(int count_threads)
        {
            set_delegate set_delegate = new set_delegate(SetToQueue);
            save_delegate save_delegate = new save_delegate(Save);
            print_delegate print_delegate = new print_delegate(Print);
            _threads = new List<Thread>();
            for (int i = 0; i < count_threads; i++)
            {
                for (int j = 0; j < count_threads; j++)
                {

                    if (i == 0)
                    {
                        thread = new Thread(new ParameterizedThreadStart(set_delegate));
                    }
                    if (i == 1)
                    {
                        thread = new Thread(new ParameterizedThreadStart(save_delegate));
                    }
                    if (i == 2)
                    {
                        thread = new Thread(new ParameterizedThreadStart(print_delegate));
                    }
                    thread.IsBackground = true;
                    thread.Name = j.ToString();
                    _threads.Add(thread);
                    _threads[j].Start(thread.Name);
                }
                _threads.Clear();
            }
        }

        Client client = new Client();
        //Метод ставит документ в очередь на печать
        public void SetToQueue(object Name)
        {
            Thread.Sleep(500);
            int currrent_name = Convert.ToInt32(Name) + 1;
            //Пoлучил документ
            _ui(client.set_document_to_print(currrent_name, true));
            Thread.Sleep(500);
        }
        //Метод сохраняет документ
        public void Save(object Name)
        {
            Thread.Sleep(1000);
            int currrent_name = Convert.ToInt32(Name) + 1;
            //Сохранил документ
            _ui(client.save_document(currrent_name, true));
        }
        //Метод печатает документ
        public void Print(object Name)
        {
            int currrent_name = Convert.ToInt32(Name) + 1;
            //Напечатал
            Thread.Sleep(2000);
            _ui(client.Print_document(currrent_name, true));
            Thread.CurrentThread.Interrupt();
        }
        //Метод завержает работу потоков при нажатии на кнопку"Отма"
        public void Abort()
        {
            foreach (Thread thread in _threads)
            {
                thread.Abort();
            }
            _threads.Clear();
        }
    }

    public class Client
    {
        public string set_document_to_print(int Name, bool success)
        {
            string approve = "";
            if (success == true)
            {
                approve = "Документ №" + Name + " поставлен в очередь";
            }

            return approve;
        }

        public string save_document(int Name, bool success)
        {
            string approve = "";
            if (success == true)
            {
                approve = "Документ №" + Name + " сохранён!";
            }

            return approve;
        }

        public string Print_document(int Name, bool success)
        {
            string approve = "";
            if (success == true)
            {
                approve = "Документ №" + Name + " напечатан!";
            }
            return approve;
        }
    }
}
