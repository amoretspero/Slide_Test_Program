module Text_functions

open System
open System.Collections
open System.Linq
open System.IO
open System.Collections.Generic
open System.Text


/// text_to_list function reads answers from specified file and returns answers in list form.
let rec text_to_list () =
    let answers = File.ReadAllLines(@"..\..\..\Answers\answers.txt", Encoding.Unicode)
    answers