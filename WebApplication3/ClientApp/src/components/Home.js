import React, { Component } from 'react';
import $ from 'jquery';
import { FileDragDrop } from './FileDragDrop';

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {
            sentences:null,
            sortedSentences:null
        };
    }

    setText = (event) => {
        this.setState({ sentences: event.target.value });
    }

    sort=(event)=> {
        let text = this.state.sentences;
        this.$_sortSentences(text);
    }

    //jQuery sort function because I prefer it over the standard js web api fetch function
    $_sortSentences = (text) => {
        $.ajax({
            type: 'post',
            url: 'api/SampleData/ParseFileSentences',
            data: { unsortedText: text },
            success: (resp) => {
                this.setState({ sortedSentences: resp });
            },
            error: (err) => {
                this.setState({ sortedSentences: 'Error: ' + err.statusText });
            }
        });
    }

    render() {
        return (
            <div>
                <h1>This is a simple sentence sorting app built with  <strong>.NET Core</strong> and <strong>React</strong></h1>
                <p>Click the "Choose file" button to get started.</p>
                <FileDragDrop />
                <button name="sortButton" onClick={this.sort}>Sort</button><br/>
                <textarea id="sorted" value={this.state.sortedSentences} />
            </div>
        );
    }
}