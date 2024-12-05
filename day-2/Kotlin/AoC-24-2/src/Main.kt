import java.io.File
import java.io.BufferedReader
import kotlin.math.abs

var totalSafeReports = 0

fun main() {
    val bufferedReader: BufferedReader = File("src/input-coa-24-2.txt").bufferedReader()
    val inputString = bufferedReader.use { it.readText() }

    val reports = inputString.split("\n")
    var totalReportsChecked = 0

    for (report in reports) {
        totalReportsChecked++
        println("\uD83E\uDD8C #$totalReportsChecked RED-NOSED REPORT " + report)
        checkIfReportSafe(report)
    }

    println("\nTOTAL SAFE REPORTS: " + totalSafeReports)
}

fun checkIfReportSafe(report: String, activatedProblemDampener: Boolean = false) {
    var isSafe = true
    val reportNumbers = report.split(" ").map { it.trim() }.filter { it.isNotEmpty() }.map { it.toInt() }
    var isIncreasing: Boolean? = null
    var prevNumber = reportNumbers[0]

    for (i in 1 until reportNumbers.size) {
        val currentNumber = reportNumbers[i]
        val difference = currentNumber - prevNumber

        if (isIncreasing == null) {
            isIncreasing = difference > 0
        }

        println("\tDifference between $prevNumber and $currentNumber is $difference. Temperature is ${if (isIncreasing) "rising" else "falling"}.")

        if (!checkSingleTemp(currentNumber, prevNumber, difference, isIncreasing)) {
            if (!activatedProblemDampener) {
                runProblemDampener(reportNumbers, i)
                return
            }
            else{
                isSafe = false
            }
        }

        prevNumber = currentNumber
        isIncreasing = difference > 0
    }

    if (isSafe == true) {
        println("\t\uD83C\uDF1F\uD83C\uDF84 Report is SAFE \uD83C\uDF84\uD83C\uDF1F\n")
        totalSafeReports++
    } else {
        println("\t\uD83E\uDD83 Report is NOT SAFE\n")
    }
}

fun checkSingleTemp(temp: Int, prevTemp: Int, difference: Int, initialTrend: Boolean): Boolean {
    if (temp == prevTemp) {
        println("\t❄\uFE0F Temperature cannot be identical!")
        return false
    }

    if (abs(difference) > 3) {
        println("\t\uD83D\uDD34 Temperature change is too volatile!!")
        return false
    }

    val currentTrend = difference > 0
    if (currentTrend != initialTrend) {
        println("\t\uD83D\uDD25 Temperature trend is inconsistent!")
        return false
    }

    return true
}

fun runProblemDampener(report: List<Int>, problemIndex: Int) {
    println("\t\uD83E\uDDE3 Activating Problem Dampener for $report")
    val dampenedReport = report.filterIndexed { index, _ -> index != problemIndex }
    val dampenedReportString = dampenedReport.joinToString(" ")

    println("\n\t\uD83E\uDD8C Rerunning dampened report: $dampenedReportString")

    return checkIfReportSafe(dampenedReportString, true)
}