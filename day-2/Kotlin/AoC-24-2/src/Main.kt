import java.io.File
import java.io.BufferedReader

fun main() {
    val bufferedReader: BufferedReader = File("src/input-coa-24-2.txt").bufferedReader()
    val inputString = bufferedReader.use { it.readText() }

    val reports = inputString.split("\n")
    var totalSafeReports = 0
    var totalReportsChecked = 0

    for (report in reports) {
        totalReportsChecked++
        println("\uD83E\uDD8C #$totalReportsChecked RED-NOSED REPORT " + report)

        if (checkIfReportSafe(report)) {
            println("\t\uD83C\uDF1F\uD83C\uDF84 Report is SAFE \uD83C\uDF84\uD83C\uDF1F\n")
            totalSafeReports++
        } else {
            println("\t\uD83E\uDD83 Report is NOT SAFE\n")
        }
    }

    println("\nTOTAL SAFE REPORTS: " + totalSafeReports)
}

fun checkIfReportSafe(report: String): Boolean {
    var isSafe = true
    val reportNumbers = report.split(" ").map { it.trim() }.filter { it.isNotEmpty() }
    var prevNumber: Int? = null
    var isIncreasing: Boolean? = null
    var difference = 0

    for (i in reportNumbers.indices) {
        val number = reportNumbers[i].toInt()

        if (prevNumber == null) {
            prevNumber = number
        } else {
            difference = (number - prevNumber)

            if (isIncreasing == null){
                isIncreasing = difference > 0
            }

            println("\tDifference between $prevNumber and $number is $difference. Temperature is ${if (isIncreasing) "rising" else "falling"}.")

            if (difference == 0) {
                println("\t❄\uFE0F There was neither an increase nor decrease in temperature!")
                isSafe = false
                break
            } else if (difference in -3..3) {
                if (isIncreasing == difference > 0) {
                    prevNumber = number
                } else {
                    println("\t\uD83E\uDDE3 Report has both increasing and decreasing temperatures!")
                    isSafe = false
                    break
                }
            } else {
                isSafe = false
                break
            }
        }
        }
    return isSafe
}