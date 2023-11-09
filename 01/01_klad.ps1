$in = get-content C:\Users\nathan.vos\OneDrive\Programmeren\AdventofCode2022\01_demo.txt

[int[]] $cals = @()
$sum = 0
$in | % {
    if($_){
        $sum+=[int]$_
    } else {
        $cals += $sum
        $sum = 0
    }
}

$cals | sort -Descending | select -First 1