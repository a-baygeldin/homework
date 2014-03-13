//Sum

let rec sum l =
    match l with
    |[] -> 0
    |hd::tl -> hd + sum tl

let list = [1;2;3;4;5;6;7;8;9;10]
    in printf "List sum: %A \n" (sum list)

//Append

let rec unite lf ls =
    match lf with
    |[] -> ls
    |hd::tl -> hd::(unite tl ls) 

let lf = [1;2;3] in
    let ls = [4;5;6] in
        let list = unite lf ls
            in printf "Unite: %A \n" list

//End

let addElem lf elem = unite lf (elem::[])
    
let elem = 4 in
    let list = [1;2;3] in
        printf "Add to end: %A\n" (addElem list elem)

//Filter

let rec filter con list =
    match list with
    |[] -> []
    |hd::tl -> if (con hd) then hd::(filter con tl) else (filter con tl)

let list = [-2;-1;0;1;2] in
    printf "Filter: %A\n" (filter (fun x -> x > 0) list)

//Sqr

let sqr =
    let rec sqrfun elem num =
        if (elem < (num / 2)) then 
            if (elem*elem < num) then 
                (elem*elem)::(sqrfun (elem + 1) num) 
            else 
                (sqrfun (elem + 1) num)
        else []
    sqrfun 1     

printf "Sqr: %A" (sqr 10)