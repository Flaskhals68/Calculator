using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt___Kalkulator
{
    public partial class Form1 : Form
    {
        List<double> lista = new List<double>();
        string prossesInput;                                                           //En variabel som lagrar inputen till processTextbox
        string tecken;
        double Svar;
        bool räknesettAnvänt = false;
        bool likamedTryckt = false;
        bool rotenuranvänt = false;
        bool EttGenomXAnvänt = false;
        bool ErrorOccurred = false;
        public Form1()
        {
            InitializeComponent();                                          
            resultatTextbox.Text = "0";                                                //Gör så att resultattextboxen har värdet 0 när brogrammet börjar
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)                          //När man trycker på en av sifferknapparna
        {
            if (ErrorOccurred == false)                                                //Så läng inte man försöker dela med 0
            {
                if (likamedTryckt == true)                                             //Om man precis har tryckt på likamed så nollställs räknaren
                {
                    likamedTryckt = false;
                    resultatTextbox.Clear();
                    lista.Clear();
                    tecken = "";
                    prossesTextbox.Text = "";
                    resultatTextbox.Text = "";
                    Svar = 0;

                }
                Button button = (Button)sender;
                if (button.Text != "0" && resultatTextbox.Text == "0" && resultatTextbox.Text != null || räknesettAnvänt)
                    resultatTextbox.Clear();                                          //Clearar resultattextboxen om det står 0
                räknesettAnvänt = false;                                              

                if (button.Text == ",")
                {
                    if (!resultatTextbox.Text.Contains(","))                          //Gör så att man bara kan ha ett kommatecken i varje tal
                    {
                        if (resultatTextbox.Text == "")                               //Eftersom att textboxen tidigare har clearats om det stod 0, och ursprungsläget är 0
                        {                                                             //så är resultattextboxen = ""
                            resultatTextbox.Text = "0" + button.Text;                 //Då lägger vi till en nolla framför kommatecknet igen
                            prossesInput += "0" + button.Text;
                        }
                        else
                        {
                            resultatTextbox.Text += button.Text;                      //Annars lägger vi bara till ett kommatecken i resultattextboxen
                            prossesInput += button.Text;
                        }
                    }
                }
                else
                {
                    lista.Add(double.Parse(button.Text));                             //Om första inmatade siffran inte är 0 så lägger vi till den i en list
                    if (lista[0] != 0)
                    {
                        resultatTextbox.Text += button.Text;
                        prossesInput += button.Text;
                    }
                    else
                    {                                   
                        lista.Remove(lista[0]);                                       //Annars tar vi bort den igen
                        resultatTextbox.Text = "0";
                    }
                }
            }
        }
        private void Operator_Click(object sender, EventArgs e)                       //När man trycker på ett av de fyra räknesetten
        {
            if (ErrorOccurred == false)                                               //Så länge man inte försöker dela med 0
            {
                if (resultatTextbox.Text != "")                                       //Så länge resultattextboxen har ett värde
                {
                    Button button = (Button)sender;                                   
                    if (räknesettAnvänt == false)                                     //Så länge man inte redan har angätt en operator.
                    {                                                                 //På så sätt tar vi bort broblemet att man kan skriva in t.ex. +*/

                        if (Svar != 0)                                                //Så länge det inte är första inmatningen
                        {
                            if (likamedTryckt == true || EttGenomXAnvänt == true)     //Om likamed är tryckt eller EttGenomX så används svaret i nästa beräkning
                            {
                                likamedTryckt = false;
                                tecken = button.Text;
                                prossesTextbox.Text += resultatTextbox.Text + " " + tecken + " ";
                                prossesInput = "";
                                räknesettAnvänt = true;
                            }
                            else if (ErrorOccurred == true)                           //Om man försökt dela med 0 så nollställs räknaren
                            {
                                ErrorOccurred = false;
                                resultatTextbox.Clear();
                                lista.Clear();
                                tecken = "";
                                prossesTextbox.Text = "";
                                resultatTextbox.Text = "";
                                Svar = 0;

                            }
                            else
                            {
                                if (rotenuranvänt == false)                          //Kollar om man precis använt rotenur
                                {

                                    switch (tecken)                                  //Kollar vilken operator som valts och använder respektive räknesett
                                    {
                                        case "+":
                                            resultatTextbox.Text = (Svar + double.Parse(resultatTextbox.Text)).ToString();
                                            break;
                                        case "-":
                                            resultatTextbox.Text = (Svar - double.Parse(resultatTextbox.Text)).ToString();
                                            break;
                                        case "*":
                                            resultatTextbox.Text = (Svar * double.Parse(resultatTextbox.Text)).ToString();
                                            break;
                                        case "/":
                                            if (resultatTextbox.Text == "0")        //Om man försöker dela med 0 så uppstår ett fel
                                            {
                                                ErrorOccurred = true;
                                                resultatTextbox.Text = "ERROR";
                                            }
                                            else
                                            {                                       //Annars fungerar det som vanligt
                                                resultatTextbox.Text = (Svar / double.Parse(resultatTextbox.Text)).ToString();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                tecken = button.Text;
                                if (prossesTextbox.Text[0] == '0' && tecken == "*") //Kollar det första tecknet i prossestextboxen för att se om det är noll och om räknesettet är gånger
                                {                                                   //På så sätt kan räknaren räkna t.ex. 0 * 3 * 9 = 0
                                    resultatTextbox.Text = "0"; 
                                    Svar = 0;
                                }
                                else
                                {
                                    Svar = double.Parse(resultatTextbox.Text);      //Svar lagrar resultattextboxen
                                }



                                if (rotenuranvänt == true)                          //Om roten ur precis har använts så skrivs inte räknesettet ut
                                {                                                   //Det är på grund av att rotenur inte räknas i Operator() utan i sin egen void
                                    prossesTextbox.Text += tecken + " ";
                                }
                                else
                                {
                                    if (prossesInput == "" || prossesInput == null) //Om prossesinput är tom så lägger vi till en nolla i prcsessTextboxen
                                    {                                               //Vi lägger även till tecknet så användaren vet vad hen räknar
                                        prossesTextbox.Text += "0 " + tecken + " ";
                                    }
                                    else
                                    {                                               //Annars lägger vi till prossesinput och tecknet
                                        prossesTextbox.Text += prossesInput + " " + tecken + " ";
                                    }
                                }
                                prossesInput = "";                                  //Prossesinput clearas inför nästa tal
                                räknesettAnvänt = true;
                                rotenuranvänt = false;
                            }
                        }
                        else                                                        //Om det är första talet som matas in
                        {
                            tecken = button.Text;                                   //Läser av vilken operator som används
                            Svar = double.Parse(resultatTextbox.Text);
                            if (rotenuranvänt == true)                              //Om rotenur har använts så läggs operatorn till i prossestextboxen
                            {
                                prossesTextbox.Text += tecken + " ";
                            }
                            if (prossesInput == "" && EttGenomXAnvänt == false || prossesInput == null && EttGenomXAnvänt == false)         //Om prossesinput är tom så lägger vi till en nolla i början av process
                            {                                                                                                               //EttGenomXAnvänt används för att inte få en extra solla i processtextboxen
                                prossesTextbox.Text += "0 " + tecken;
                            }
                            else                                                    //Annars så använder vi oss av prossesinput
                            {
                                prossesTextbox.Text += prossesInput + " " + tecken + " ";
                            }
                            prossesInput = "";
                            räknesettAnvänt = true;
                        }
                    }
                    else                                                            //Om användaren redan har valt ett räknesätt
                    {                                                               //Då ändrar vi tecknet och och tar bort de två sista tecknerna i processinput
                                                                                    //Alltså vi tar bort operatorn i prossesinput och byter ut den mot ett nytt 
                        prossesTextbox.Text = prossesTextbox.Text.Remove(prossesTextbox.Text.Length - 2);
                        Svar = double.Parse(resultatTextbox.Text);
                        tecken = button.Text;
                        prossesTextbox.Text += tecken + " ";
                        räknesettAnvänt = true;
                    }
                }
                lista.Clear();                                                      //Listan clearas inför nästa tal
            }
            
        }
        private void Likamed_Click (object sender, EventArgs e)
        {
            if (resultatTextbox.Text != "")                                         //Så länge inte resultattextboxen är tom
            {
                if (rotenuranvänt == false)                                         //Så länge inte rotenur har använts
                {
                    switch (tecken)                                                 //Läser av vad operatorn är och använder respektive räknesett
                    {
                        case "+":
                            resultatTextbox.Text = (Svar + double.Parse(resultatTextbox.Text)).ToString();
                            break;
                        case "-":
                            resultatTextbox.Text = (Svar - double.Parse(resultatTextbox.Text)).ToString();
                            break;
                        case "*":
                            resultatTextbox.Text = (Svar * double.Parse(resultatTextbox.Text)).ToString();
                            break;
                        case "/":
                            if (resultatTextbox.Text == "0")                        //Om man försöker dela med 0 så skrivs "ERROR" ut
                            {
                                resultatTextbox.Text = "ERROR";
                                ErrorOccurred = true;
                            }
                            else                                                   //Annars fungerar det som vanligt
                            {
                                resultatTextbox.Text = (Svar / double.Parse(resultatTextbox.Text)).ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (prossesTextbox.Text == "")                                   //Kollar om prossesTextboxen är tom
                {                                                                //Det gör för att motverka fel i den kommande else if satsen
                    resultatTextbox.Text = resultatTextbox.Text;
                }
                else if (prossesTextbox.Text[0] == '0' && tecken == "*")         //Kollar det första tecknet i prossestextboxen för att se om det är noll och om räknesettet är gånger
                {
                    resultatTextbox.Text = "0";
                    Svar = 0;
                }
                else
                {
                    Svar = double.Parse(resultatTextbox.Text);                   //Lagrar resultatTextboxen i en variabel
                }
                if (ErrorOccurred == false)                                      //Om inte ett försök att dela med 0 har gjorts så clearars processtextboxen
                {
                    prossesTextbox.Clear();
                }
                lista.Clear();
                prossesInput = "";
                likamedTryckt = true;
                räknesettAnvänt = false;
            }
        }
        private void ButtonC_Click(object sender, EventArgs e)                  //Clearar allt och nollställer räknaren
        {
            likamedTryckt = false;
            räknesettAnvänt = false;
            ErrorOccurred = false;
            lista.Clear();
            resultatTextbox.Text = "0";
            prossesTextbox.Clear();
            tecken = "";
            prossesInput = "";
            Svar = 0;
        }

        private void ButtonCE_Click(object sender, EventArgs e)                 //Clearar det du precis har skrivit in
        {
            
            resultatTextbox.Text = "0";
            if (ErrorOccurred == true)                                          //Om man har försökt dela med 0 och ButtonCE trycks så clearas allt
            {
                likamedTryckt = false;
                räknesettAnvänt = false;
                lista.Clear();
                prossesTextbox.Clear();
                tecken = "";
                prossesInput = "";
                Svar = 0;
            }
            ErrorOccurred = false;
        }
        
        private void ButtonPlusMinus_Click(object sender, EventArgs e)         //Multiplicerar resultattextboxen och prossesinput med -1
        {
            resultatTextbox.Text = (double.Parse(resultatTextbox.Text) * -1).ToString();
            prossesInput = (double.Parse(prossesInput) * -1).ToString();
        }

        private void Backspace_Click(object sender, EventArgs e)              //Tar bort det senaste siffran som skrevs in
        {                                                                     //Om resultattextboxen har ett värde
            if (resultatTextbox.Text.Length >= 0 && likamedTryckt == false && prossesInput != null)
            {                                                                 //Tar bort det sista tecknet i resultattextboxen
                resultatTextbox.Text = resultatTextbox.Text.Remove(resultatTextbox.Text.Length - 1);
                if (resultatTextbox.Text.Length == 0)                         //Om längden på resultattextbox = 0 så lägger vi in "0" i textboxen
                {
                    resultatTextbox.Text = "0";
                }
                if (prossesInput.Length > 0)                                  //Om längden på prossesinput är större eller lika med 0
                {                                                             //så tar den bort det sista värdet i prossesinput
                    prossesInput = prossesInput.Remove(prossesInput.Length - 1);
                    if (lista.Count > 0)
                    {
                        lista.Remove(lista[lista.Count - 1]);
                    }
                }
                
            }
        }
        private void EttGenomX_Click(object sender, EventArgs e)                            //När man trycker på "1/X" knappen
        {                                                                     
            prossesTextbox.Text += "reciproc ( " + resultatTextbox.Text + " ) ";            //Skriver ut det inmatade värdet i processtextboxen
            resultatTextbox.Text = (1 / double.Parse(resultatTextbox.Text)).ToString();     //Delar 1 på det inmatadevärdet och skriver ut det i resultattextboxen
            prossesInput = "";
            lista.Clear();                                                                  //Ränsar prossesinput och lista för senare användning
            EttGenomXAnvänt = true;                                                         //En bool som visar på att EttGenomX har använts
        }
        
        private void ButtonSquareRoot_Click(object sender, EventArgs e)                     //När man räknar med rotenur
        {
            rotenuranvänt = true;                                                           //En bool som visar att man precis har räknat med rotenur
                                                                                            //Den används för att inte räknaren ska räkna dubbelt när man väljer
                                                                                            //en operator

            prossesTextbox.Text += "sqrt( " + resultatTextbox.Text + " ) ";                 //Det inmatade skrivs ut i processtextboxen
                                                                                            //Räknaren drar roten ur vad som är inmatat
            resultatTextbox.Text = resultatTextbox.Text = Math.Sqrt(double.Parse(resultatTextbox.Text)).ToString();
            switch (tecken)                                                                 //Gör en uträkning beroende på valt räknesett
            {
                case "+":
                    resultatTextbox.Text = (Svar + double.Parse(resultatTextbox.Text)).ToString();
                    break;
                case "-":
                    resultatTextbox.Text = (Svar - double.Parse(resultatTextbox.Text)).ToString();
                    break;
                case "*":
                    resultatTextbox.Text = (Svar * double.Parse(resultatTextbox.Text)).ToString();
                    break;
                case "/":
                    resultatTextbox.Text = (Svar / double.Parse(resultatTextbox.Text)).ToString();
                    break;
            }
            Svar = double.Parse(resultatTextbox.Text);                                      //Resultattextboxen lagras i en variabel
        }
    }
}
