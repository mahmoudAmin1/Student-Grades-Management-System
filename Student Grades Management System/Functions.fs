module Student_Grades_Management_System.Functions

open System.IO

let readFileTo2DArray filePath =
    // Check if the file exists
    if File.Exists(filePath) then
        // Read all lines from the file
        let lines = File.ReadAllLines(filePath)
        
        // Convert each line into an array 
        let textArray = 
            lines 
            |> Array.map (fun line -> line.Split(','))  // Split each line by comma
        
        textArray  // Return the 2D array
    else
        // Return an empty array if the file doesn't exist
        [||]
let appendToFile filePath content =
    File.AppendAllText(filePath, "\n" + content)

let users = readFileTo2DArray "../../../Files/users.txt"
let students=readFileTo2DArray "../../../Files/students.txt"

let Studentsdata () =
   (students)
let login (username:string) (password:string) =
    match Array.tryFind (fun (user:string[]) -> user.[0] = username && user.[1] = password) users with
    | Some user -> (user.[2])
    | None -> "Invalid username or password."
let addStudent (id:string)(name:string)(grade1:string)(grade2:string)(grade3:string) =
    match Array.tryFind (fun (student:string[]) -> student.[0] = id) students with
    | Some _ -> "ID Is Allready Taken."
    | None ->
        let newStudentLine = sprintf "%s,%s,%s,%s,%s" id name grade1 grade2 grade3
        appendToFile "../../../Files/students.txt" newStudentLine
        "Student added successfully."
