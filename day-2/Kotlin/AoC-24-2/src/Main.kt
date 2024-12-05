import java.io.File
import java.io.BufferedReader
fun main() {
    val bufferedReader: BufferedReader = File("src/input-coa-24-2.txt").bufferedReader()
    val inputString = bufferedReader.use { it.readText() }
    println(inputString)
}