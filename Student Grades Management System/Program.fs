//namespace Student_Grades_Management_System

open System
open System.Windows.Forms
open System.Drawing
open Student_Grades_Management_System


[<STAThread>]
[<EntryPoint>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    let form = new MainForm()
    Application.Run(form)
    0
