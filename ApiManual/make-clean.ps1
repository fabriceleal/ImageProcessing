# Clean


[Reflection.Assembly]::loadWithPartialName("system.io")

$baseDir = [system.io.path]::GetDirectoryName( $myinvocation.MyCommand.Definition )

$out = "out"
$out = [System.io.path]::combine( $baseDir , $out )

$conf = "doxygen_warnings.txt"
$conf = [System.io.path]::combine( $baseDir , $conf )

if ([System.io.directory]::Exists($out)){
	[System.io.directory]::Delete($out, $true)
}

if ([System.io.file]::Exists($conf)){
	[System.io.file]::Delete($conf)
}
