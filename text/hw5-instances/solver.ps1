# initial config
# a - is experiment identifier
# v - verbose
# others - GA params
param ([int] $g, [int] $p, [int] $t, [int] $e, [double] $m, [string] $a, [bool] $v = 0)

$program = 'C:\Users\tomas.svatek\source\repos\SATProblem\SATProblem'

get-childitem 'C:\Users\tomas.svatek\Desktop\hw5-instances\wuf20-78-M1' | % {
    Write-Host "Running $_ with config: g:$g::p:$p::t:$t::e:$e::m:$m"
    $programResult = dotnet run --configuration Release --project $program --f $_.FullName --g $g --p $p --t $t --e $e --m $m

    $result = "ExpId:$a;G:$g;P:$p;T:$t;E:$e;M:$m;$programResult"
    $result | Out-File -FilePath .\result.txt -Append
}

get-childitem 'C:\Users\tomas.svatek\Desktop\hw5-instances\wuf50-201-R1' | % {
    Write-Host "Running $_ with config: g:$g::p:$p::t:$t::e:$e::m:$m"
    $programResult = dotnet run --configuration Release --project $program --f $_.FullName --g $g --p $p --t $t --e $e --m $m

    $result = "ExpId:$a;G:$g;P:$p;T:$t;E:$e;M:$m;$programResult"
    $result | Out-File -FilePath .\result.txt -Append
}