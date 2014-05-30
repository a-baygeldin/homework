(*
A. Baygeldin (c) 2014
Web'CPS
*)

module ImageParser

open WebR
open System

let urls = ["http://msdn.microsoft.com";"https://duckduckgo.com"; "https://www.bing.com"]

let rec map f l cps =
    match l with
    | hd :: tl -> f hd (fun x -> map f tl (fun y -> cps (x :: y)))
    | [] -> cps []

let countImg (s:string) = (s.Length - s.Replace("<img", "").Length) / "<img".Length

let rec parseImg (s:string) = 
    let img = s.IndexOf("<img")
    if img = -1 then [] else
        let linkStart = s.IndexOf("src=\"", img)
        let linkEnd = s.IndexOf("\"", linkStart + 5)
        s.Substring(linkStart + 5, linkEnd - linkStart - 5) :: parseImg (s.Remove(0, linkEnd))

let cps list = printfn "%A" (Seq.toList (Seq.distinct (Seq.fold (fun acc x -> (parseImg x) @ acc) [] (Seq.filter (fun x -> countImg x <= 5) list))))

map getUrl urls cps

Console.ReadLine() |> ignore