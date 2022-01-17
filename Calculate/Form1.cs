using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculate
{
    public partial class Form1 : Form
    {
        char[] operators = { '+', '-', '*', '/' };
        List<string> numbers = new List<string>();
        Stack<char> operators_stack = new Stack<char>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResultTextBox.SelectAll();
            ResultTextBox.SelectionAlignment = HorizontalAlignment.Right;
        }

        private char Read_button_text(object sender, EventArgs e)
        {
            char text_from_button = (sender as Button).Text[0];
            return text_from_button;
        }

        private void Add_Text_from_button(object sender, EventArgs e)
        {
            char temp = Read_button_text(sender, e);
            if (operators.Contains(temp))
            {
                Correct_sign();
            }
            ResultTextBox.Text += temp;
        }

        private char last_char_from_resultbox()
        {
            return ResultTextBox.Text.Last();
        }

        private void Correct_sign()
        {
            if (!Char.IsNumber(last_char_from_resultbox()) & last_char_from_resultbox() != '!' & last_char_from_resultbox() != 'e' & last_char_from_resultbox() != 'п')
            {
                String temp = ResultTextBox.Text.TrimEnd(last_char_from_resultbox());
                ResultTextBox.Text = temp;
            }
        }

        private void BtnClearResultBox_Click(object sender, EventArgs e)
        {
            numbers.Clear();
            operators_stack.Clear();
            ResultTextBox.Text = "";
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if(ResultTextBox.Text == "e")
            {
                ResultTextBox.Text = Math.E.ToString();
            }
            else if(ResultTextBox.Text == "п")
            {
                ResultTextBox.Text = Math.PI.ToString();
            }
            else if(ResultTextBox.Text == "пe" | ResultTextBox.Text == "eп")
            {
                ResultTextBox.Text = (Math.PI * Math.E).ToString();
            }
            else if(ResultTextBox.Text.Contains("/0"))
            {
                MessageBox.Show("На нуль ділити не можна!");
            }
            else
            {
                Correct_sign();
                Parse_String_Result();
                Show_Polish_system();
            }
        }

        private void Parse_String_Result()
        {
            for (int i = 0; i < ResultTextBox.Text.Length; i++)
            {
                if (Char.IsNumber(ResultTextBox.Text[i]) | ResultTextBox.Text[i] == ',')
                {
                    string temp = "";
                    while (i < ResultTextBox.Text.Length && Char.IsNumber(ResultTextBox.Text[i]) | ResultTextBox.Text[i] == ',')
                    {
                        temp += ResultTextBox.Text[i];
                        i++;
                    }
                    i--;
                    numbers.Add(temp);
                }
                else
                {
                    Control_correct_stack_symbols(ResultTextBox.Text[i]);
                }
            }
            FromStack_to_Numbers();
        }

        private void FromStack_to_Numbers()
        {
            foreach (char st in operators_stack)
            {
                numbers.Add(st.ToString());
            }
            operators_stack.Clear();
        }

        private void Control_correct_stack_symbols(char oper)
        {
            if (operators_stack.Count > 0)
            {
                int value_new_operator = Get_Value_Operator(oper);
                int value_old_operator = Get_Value_Operator(operators_stack.Peek());
                if (value_new_operator > value_old_operator)
                {
                    operators_stack.Push(oper);
                }
                else if (value_new_operator < value_old_operator)
                {
                    FromStack_to_Numbers();
                    operators_stack.Push(oper);
                }
                else if (value_new_operator == value_old_operator)
                {
                    char c = operators_stack.Pop();
                    string s = c.ToString();
                    numbers.Add(s);
                    operators_stack.Push(oper);
                }
            }
            else
            {
                operators_stack.Push(oper);
            }
        }

        private int Get_Value_Operator(char char_change)
        {
            int value_operator = 0;
            switch (char_change)
            {
                case '+':
                    value_operator = 1;
                    break;
                case '-':
                    value_operator = 1;
                    break;
                case '*':
                    value_operator = 2;
                    break;
                case '/':
                    value_operator = 2;
                    break;
                case '!':
                    value_operator = 3;
                    break;
                case 'e':
                    value_operator = 2;
                    break;
                case 'п':
                    value_operator = 2;
                    break;
                default:
                    break;
            }
            return value_operator;
        }

        private void Special_symbols_text_from_textbox(object sender, EventArgs e)
        {
            string temp = (sender as Button).Text;
            if(temp == "x!")
            {
                ResultTextBox.Text += '!';
            }
            else if(temp == "PI")
            {
                ResultTextBox.Text += 'п';
            }
            else if (temp == "e")
            {
                ResultTextBox.Text += 'e';
            }
        }

        private float factorial(float number)
        {
            float temp = 1;
            for(int i = 1; i <= number; i++)
            {
                temp *= i;
            }
            return temp;
        }

        private float Process_calculate_polish_system()
        {
            Stack<float> final_result_stack = new Stack<float>();
            foreach (string ass in numbers)
            {
                if (Char.IsNumber(ass[0]))
                {
                    float a;
                    float.TryParse(ass, out a);
                    final_result_stack.Push(a);
                }
                else
                {
                    char test_ = ass[0];
                    float first = final_result_stack.Pop();
                    float second = 0;
                    if(test_ == 'п')
                    {
                        first *= (float)Math.PI;
                        second = first;
                    }
                    else if (test_ == 'e')
                    {
                        first *= (float)Math.E;
                        second = first;
                    }
                    else if (test_ == '!')
                    {
                        first = factorial(first);
                        second = first;
                    }
                    else if (test_ == '+')
                    {
                        second = final_result_stack.Pop();
                        second += first;
                    }
                    else if (test_ == '*')
                    {
                        second = final_result_stack.Pop();
                        second *= first;
                    }
                    else if (test_ == '-')
                    {
                        second = final_result_stack.Pop();
                        second -= first;
                    }
                    else if (test_ == '/')
                    {
                        second = final_result_stack.Pop();
                        if (first / second == 0)
                        {
                            MessageBox.Show("На нуль ділити не можна!");
                        }
                        else
                        {
                            second /= first;
                        }
                    }
                    final_result_stack.Push(second);
                }
            }
            return final_result_stack.Pop();
        }

        private void Show_Polish_system()
        {
            ResultTextBox.Text = Process_calculate_polish_system().ToString();
        }
    }
}