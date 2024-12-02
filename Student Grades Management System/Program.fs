open System
open System.Windows.Forms
open Student_Grades_Management_System.MainForm


[<STAThread>]
[<EntryPoint>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    let form = new MainForm()
    Application.Run(form)
    0
