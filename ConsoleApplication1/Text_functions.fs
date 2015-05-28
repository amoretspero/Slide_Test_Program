module Text_functions

open System
open System.Collections
open System.Linq
open System.IO
open System.Collections.Generic
open System.Text


/// text_to_list function reads answers from specified file and returns answers in list form.
let rec text_to_list () =
    let answers_dir = new DirectoryInfo(@"..\..\..\Slide_Test_Program_Answers")
    let answers_dir_release = new DirectoryInfo(@".\Slide_Test_Program_Answers")
    if answers_dir.Exists then
        //let answers = File.ReadAllLines(@"..\..\..\Answers\answers.txt", Encoding.Unicode)
        let answers = File.ReadAllLines(@"..\..\..\Slide_Test_Program_Answers\answers.txt", Encoding.Unicode)
        answers
    else
        let answers = File.ReadAllLines(@".\Slide_Test_Program_Answers\answers.txt", Encoding.Unicode)
        answers