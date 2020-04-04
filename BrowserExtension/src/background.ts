function showBadge(text: string, color: string) {
  chrome.browserAction.setBadgeText({ text });
  chrome.browserAction.setBadgeBackgroundColor({ color });
}

function showLostConnectivity() {
  showBadge('!', 'red');
}

function showConnectivity() {
  showBadge('ok', 'green');
}

chrome.runtime.onMessage.addListener((request) => {
  if (request.connectionLost) {
    showLostConnectivity();
  } else if (request.connected) {
    showConnectivity();
  }
});
