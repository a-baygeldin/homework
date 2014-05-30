(*
A. Baygeldin (c) 2014
ChatBot with WinForms
*)
//в фспродж прописать что надо. разобраться с дллкой
module ChatBot

open System
open System.Drawing
open System.Windows.Forms
open AIMLbot

let myBot = new Bot()
myBot.loadSettings()
let myUser = new AIMLbot.User(Environment.UserName, myBot);
myBot.loadAIMLFromFiles();

let form = new Form(Text = "Stallman Bot", Height = 400, Width = 300)
let message = new TextBox()
let conversation = new TextBox()
let bt1 = new Button()

let enter (message:#TextBox) (conversation:#TextBox) (e:KeyEventArgs) =
    if (e.KeyCode = Keys.Enter) then 
        if message.Text = "Bye" then
            conversation.AppendText("Stallman: See you soon!\n")
            System.Threading.Thread.Sleep(1000)
            Application.Exit()
        else
            let request = new Request(message.Text, myUser, myBot)
            let answer = myBot.Chat(request)
            conversation.AppendText(Environment.UserName + ": " + message.Text + "\n")
            conversation.AppendText("Stallman: " + answer.Output.ToString() + "\n")
            message.Clear()

let rnd = new Random()
let r, g, b = rnd.Next(256), rnd.Next(256), rnd.Next(256)

form.Icon <- new System.Drawing.Icon("favicon.ico")
message.Dock <- DockStyle.Bottom
conversation.Dock <- DockStyle.Fill
conversation.Multiline <- true
conversation.ScrollBars <- ScrollBars.Vertical
conversation.WordWrap <- true
conversation.ReadOnly <- true
conversation.BackColor <- Color.FromArgb(r, g, b)
bt1.Dock <- DockStyle.Bottom
bt1.Image <- new Bitmap("favicon.ico");
bt1.ImageAlign <- System.Drawing.ContentAlignment.MiddleCenter;

form.Controls.Add(conversation)
form.Controls.Add(message)
form.Controls.Add(bt1)
form.Show()

message.Focus() |> ignore
message.KeyDown.Add(enter message conversation)
message.KeyDown.Add(fun e -> if e.KeyCode = Keys.Escape then Application.Exit())
bt1.Click.Add(fun _ -> conversation.AppendText("Stallman: Awesome fingernail! Thanks! :)\n"))

Application.Run()