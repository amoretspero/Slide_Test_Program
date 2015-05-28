module List_functions

open System
open System.Linq
open System.Collections
open System.Collections.Generic

/// Random list generator - Generates list of random numbers, 0 to (num-1), no duplicates.
let rec random_list_gen (num : int) (lst : int list) (lst_init : int list) =
    match lst with
    [] -> 
        let number_generated = (Random().Next())%num
        (random_list_gen (num-1) [ lst_init.[number_generated] ] (List.ofSeq((lst_init.Except([lst_init.[number_generated]])))))
    | h :: t ->
        (*if (lst.Length <> num) then
            let mutable temp = (Random().Next())%num
            while (lst.Contains(temp)) do
                temp <- (Random().Next())%num
            (random_list_gen num (List.append lst [temp]))
        else
            lst*)
        if (num > 0) then
            let mutable temp = (Random().Next())%num
            (random_list_gen (num-1) (List.append lst [lst_init.[temp]]) (List.ofSeq((lst_init.Except([lst_init.[temp]])))))
        else
            lst