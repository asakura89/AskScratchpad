/* 
// Define the maximum number of retries
const MAX_RETRIES = 3;

// Define the initial retry interval in milliseconds
const INITIAL_RETRY_INTERVAL = 1000;

// Define the resume delay in milliseconds
const RESUME_DELAY = 5000;

// Define the abort controller
let controller = new AbortController();

// Define the signal
let signal = controller.signal;

// Define the retry count and the retry interval
let retryCount = 0;
let retryInterval = INITIAL_RETRY_INTERVAL;

// Define the timer id
let timerId;

// Define the resume button
let resumeButton = document.getElementById("resume-button");

// Define the retry operation function
async function retryOperation() {
  try {
    // Check if the operation is canceled
    if (signal.aborted) {
      throw new Error("The operation was canceled.");
    }

    // Perform the operation
    await doWork();

    // If the operation succeeds, clear the timer and hide the resume button
    console.log("The operation succeeded.");
    clearTimeout(timerId);
    resumeButton.style.display = "none";
  } catch (error) {
    // If the operation fails, increment the retry count
    retryCount++;

    // Check if the maximum number of retries is reached
    if (retryCount > MAX_RETRIES) {
      // If yes, rethrow the error
      throw error;
    }

    // If no, log the error and wait for the next retry
    console.log("The operation failed: " + error.message);
    console.log("Waiting " + retryInterval + " ms before next retry.");

    // Use setTimeout to wait for the retry interval
    // You can also use other methods to implement the wait logic
    await new Promise((resolve) => setTimeout(resolve, retryInterval));

    // Update the retry interval with an exponential backoff
    // You can also use other methods to implement the backoff logic
    retryInterval = retryInterval * 2;

    // Retry the operation
    retryOperation();
  }
}

// Define the function that performs the operation
// You can replace this with your own logic
async function doWork() {
  // Simulate some work that may fail randomly
  // You can replace this with your own logic
  let result = Math.floor(Math.random() * 10);
  await new Promise((resolve) => setTimeout(resolve, 1000));
  if (result < 5) {
    throw new Error("Something went wrong.");
  }
}

// Define the function that cancels the operation
function cancelOperation() {
  // Cancel the abort controller
  controller.abort();

  // Show the resume button
  resumeButton.style.display = "block";
}

// Define the function that resumes the operation
function resumeOperation() {
  // Reset the abort controller
  controller = new AbortController();
  signal = controller.signal;

  // Resume the operation after a delay
  setTimeout(retryOperation, RESUME_DELAY);
}

// Start the retry operation
retryOperation();

// Set a timer to cancel the operation after a timeout
// You can adjust the timeout value as needed
timerId = setTimeout(cancelOperation, 10000);

// Add a click event listener to the resume button
resumeButton.addEventListener("click", resumeOperation);
 */