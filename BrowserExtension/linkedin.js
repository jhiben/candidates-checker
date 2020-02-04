const _PROFILE_MATCH =
  '.pv-top-card-v3--list > li.t-24,.pv-top-card--list > li.t-24,.profile-info > h1.searchable';
const _SEARCH_RESULT_MATCH = '.name.actor-name,.search-result-profile-link';
const _SEARCH_POPUP_RESULT_MATCH =
  '.basic-typeahead__triggered-content.search-global-typeahead__content.search-box_focus .typeahead-suggestion .search-typeahead-v2__hit-info > span';
const _SEARCH_RECENT_MATCH =
  '.search-typeahead-v2__history-list-carousel > li .typeahead-suggestion--carousel-item-text';

const _ALL = [
  _PROFILE_MATCH,
  _SEARCH_RESULT_MATCH,
  _SEARCH_POPUP_RESULT_MATCH,
  _SEARCH_RECENT_MATCH,
].join(',');

checker(_ALL);
