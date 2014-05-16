(*
A. Baygeldin (c) 2014
Map'CPS
*)

let negative x f = f -x;

let rec map f l cps =
    match l with
    | hd :: tl -> f hd (fun x -> map f tl (fun y -> cps (x :: y)))
    | [] -> cps []

map negative [1; 2] (printfn "%A")